using Godot;

namespace Hurtman.Actors.Components;

[GlobalClass, Tool]
public partial class PhysicalCharacter : Node, IActorComponent, IMovement3D
{


	[Export] public float MaxSpeed { get; set; }
	[Export] public float Acceleration { get; set; }
	[Export] public float MaxAccelerationForce { get; set; }
	[Export] public Curve AccelerationCurve { get; set; }

	private Vector3 _targetVelocity;

	
	public Vector3 MoveDirection { get; set; }
	public IPhysicsComponent PhysicsComponent { get; set; }
	public Actor Actor { get; set; }

	public void PhysicsTick(float delta)
	{
		MoveCharacter(delta);
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
