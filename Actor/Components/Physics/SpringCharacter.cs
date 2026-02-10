using Godot;

namespace Hurtman.Actor.Components.Physics;
[GlobalClass, Tool]
public partial class SpringCharacter : Node, IActorComponent, IMovement3D
{
	
	[Export] public float RideHeight { get; set; } = 2.0f;
	[Export] public float RideSpringStrength { get; set; } = 50.0f;
	[Export] public float RideSpringDamper { get; set; } = 5.0f;


	public void MoveInDirection(Vector3 direction)
	{
	}

	public IPhysicsComponent PhysicsComponent { get; set; }
	
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

	public IActor Actor { get; set; }
	public void PhysicsTick(float delta)
	{
		SpringFloat(delta);
	}



	public void Setup()
	{
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent>();
	
	}


	public void ProcessTick(float delta)
	{
		
	}
}
