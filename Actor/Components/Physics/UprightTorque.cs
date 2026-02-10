using Godot;

namespace Hurtman.Actor.Components;

[GlobalClass, Tool]
public partial class UprightTorque : Node, IActorComponent, IMovement3D
{	
	
	[Export]
	public float TorqueStrength = 50.0f;
	
	[Export]
	public float TorqueDamping = 0.5f;
	
	public IActor Actor { get; set; }
	public void Setup()
	{
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent>();
	}

	public void PhysicsTick(float delta)
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


		var torque = rotationAxis * angle * TorqueStrength;

		// Apply damping based on current angular velocity
		var damping = -PhysicsComponent.AngularVelocity * TorqueDamping;

		PhysicsComponent.ApplyTorque(torque + damping);
	}

	public void ProcessTick(float delta)
	{
	}

	public void MoveInDirection(Vector3 direction)
	{
	}

	public IPhysicsComponent PhysicsComponent { get; set; }
}
