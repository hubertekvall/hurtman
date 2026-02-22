using Godot;

namespace Hurtman.Actors.Components;

[GlobalClass, Tool]
public partial class UprightTorque : Node, IActorComponent, IMovement3D
{	
	
	[Export]
	public float TorqueStrength = 50.0f;
	
	[Export]
	public float TorqueDamping = 0.5f;


	[Export] public float MaxLeanAngleDegrees { get; set; } = 30.0f;

	public Actor Actor { get; set; }
	public void Setup()
	{
		PhysicsComponent3D = Actor.GetComponent<IPhysicsComponent3D>();
	}

	public void PhysicsTick(float delta)
	{
		var currentUp = PhysicsComponent3D.GlobalTransform.Basis.Y;
		var desiredUp = Vector3.Up;

		var rotationAxis = currentUp.Cross(desiredUp);
		var rotationMagnitude = rotationAxis.Length();

		if (rotationMagnitude < 0.001f)
			return;

		rotationAxis = rotationAxis.Normalized();

		var angle = Mathf.Asin(Mathf.Clamp(rotationMagnitude, -1f, 1f));
		var maxLeanAngleRad = Mathf.DegToRad(MaxLeanAngleDegrees);

		// Clamp the target angle — if we're within the limit, target is 0 (fully upright).
		// If we're past the limit, only correct back to the boundary.
		var targetAngle = Mathf.Max(0f, angle - maxLeanAngleRad);

		if (targetAngle < 0.001f)
			return;

		var torque = rotationAxis * targetAngle * TorqueStrength;
		var damping = -PhysicsComponent3D.AngularVelocity * TorqueDamping;

		PhysicsComponent3D.ApplyTorque(torque + damping);
	}

	public void ProcessTick(float delta)
	{
	}

	public void MoveInDirection(Vector3 direction)
	{
	}

	public IPhysicsComponent3D PhysicsComponent3D { get; set; }
}
