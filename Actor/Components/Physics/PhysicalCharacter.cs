using Godot;

namespace Hurtman.Actor.Components;

[GlobalClass, Tool]
public partial class PhysicalCharacter : Node, IActorComponent, IMovement3D
{
	[Export] public float RideHeight { get; set; } = 2.0f;
	[Export] public float RideSpringStrength { get; set; } = 50.0f;
	[Export] public float RideSpringDamper { get; set; } = 5.0f;


	[Export] public float MaxSpeed { get; set; }
	[Export] public float Acceleration { get; set; }
	[Export] public float MaxAccelerationForce { get; set; }
	[Export] public Curve AccelerationCurve { get; set; }

	private Vector3 _targetVelocity;
	public Vector3 MoveDirection { get; set; }
	public IPhysicsComponent PhysicsComponent { get; set; }
	public IActor Actor { get; set; }

	public void PhysicsTick(float delta)
	{
		MoveCharacter(delta);
		TorqueUpright(delta);
		SpringFloat(delta);
	}

	private void TorqueUpright(float delta)
	{
		// Get the current up direction of the rigidbody
		var currentUp = PhysicsComponent.GlobalTransform.Basis.Y;

		// Desired up direction (world up)
		var desiredUp = Vector3.Up;

		// Calculate the axis of rotation needed using cross product
		var rotationAxis = currentUp.Cross(desiredUp);

		// The magnitude of the cross product tells us how far we are from upright
		// (sin of the angle between the vectors)
		var rotationMagnitude = rotationAxis.Length();

		// If we're already upright (or very close), don't apply torque
		if (rotationMagnitude < 0.001f)
			return;

		// Normalize the axis
		rotationAxis = rotationAxis.Normalized();

		// Calculate the angle between current and desired up
		var angle = Mathf.Asin(Mathf.Clamp(rotationMagnitude, -1f, 1f));

		// Apply torque proportional to the angle
		var torqueStrength = 50f; // Adjust this value to control how strongly it rights itself
		var dampingFactor = 0.3f; // Damping to prevent oscillation

		var torque = rotationAxis * angle * torqueStrength;

		// Apply damping based on current angular velocity
		var damping = -PhysicsComponent.AngularVelocity * dampingFactor;

		PhysicsComponent.ApplyTorque(torque + damping);
	}

	private void MoveCharacter(float delta)
	{
		var moveDirection = MoveDirection.Normalized();
		var unitVel = _targetVelocity.Normalized();
		float velocityDotProduct = unitVel.Dot(PhysicsComponent.Velocity.Normalized());
		float acceleration = Acceleration * AccelerationCurve.Sample(velocityDotProduct);
		var goalVelocity = moveDirection * MaxSpeed;
		_targetVelocity = _targetVelocity.MoveToward(goalVelocity, acceleration * delta);

		var currentHorizontalVel = new Vector3(PhysicsComponent.Velocity.X, 0, PhysicsComponent.Velocity.Z);
		var maxAcceleration = MaxAccelerationForce * AccelerationCurve.Sample(velocityDotProduct);
		var neededAcceleration = ((_targetVelocity - currentHorizontalVel) / delta).LimitLength(maxAcceleration);

		PhysicsComponent.ApplyForce(neededAcceleration);
	}


	private void SpringFloat(float delta)
	{
		// Raycast downward from the physics component
		var spaceState = PhysicsComponent.GetWorld3D().DirectSpaceState;
		var rayOrigin = PhysicsComponent.GlobalTransform.Origin;
		var rayEnd = rayOrigin + Vector3.Down * (RideHeight * 2); // Cast twice the ride height

		var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
		query.CollideWithBodies = true;
		query.CollideWithAreas = false;
		query.Exclude = [PhysicsComponent.GetRid()]; // Don't hit self

		var result = spaceState.IntersectRay(query);

		if (result.Count == 0) return; // No ground detected

		var hitDistance = rayOrigin.DistanceTo((Vector3)result["position"]);
		var hitPosition = (Vector3)result["position"];
		
		
		// Calculate spring force
		var rayDirection = Vector3.Down;
		var otherVelocity = Vector3.Zero; // Assume ground is static (could get from hit rigidbody if needed)
		
		RigidBody3D hitRigidBody = null;
		if (result.ContainsKey("collider"))
		{
			var collider = result["collider"].As<Node>();
			if (collider is RigidBody3D rigidBody)
			{
				hitRigidBody = rigidBody;
				otherVelocity = rigidBody.LinearVelocity;
			}
		}
		
		var rayDirectionVelocity = Vector3.Down.Dot(PhysicsComponent.Velocity);
		var otherDirectionVelocity = Vector3.Down.Dot(otherVelocity);
		var relativeVelocity = rayDirectionVelocity - otherDirectionVelocity;

		var compressionDistance = RideHeight - hitDistance;

		// Spring force = strength * compression - damping * relative velocity
		var springForce = (compressionDistance * RideSpringStrength) - (PhysicsComponent.Velocity.Y * RideSpringDamper);

		// Apply force upward
		PhysicsComponent.ApplyForce(Vector3.Up * springForce);
		
		if (hitRigidBody != null)
		{
			// Force magnitude depends on our mass and the spring force
			var forceToApply = Vector3.Down * springForce;
		
			// Apply force at the hit position for realistic torque
			hitRigidBody.ApplyForce(forceToApply, hitPosition - hitRigidBody.GlobalPosition);
		}
		
	}






	

	public void Setup()
	{
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent>();
	}

	public void MoveInDirection(Vector3 direction)
	{
		MoveDirection = direction;
	}

	public void ProcessTick(float delta)
	{
	}
}
