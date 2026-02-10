using Godot;

namespace Hurtman.Actor.Components;

[GlobalClass, Tool]
public partial class PhysicalCharacterController : Node, IActorComponent, IMovement3D
{
	
	[Export] public float MaxSpeed { get; set; }
	[Export] public float Acceleration { get; set; }
	[Export] public float MaxAccelerationForce { get; set; }
	[Export] public Curve AccelerationCurve { get; set; }

	private Vector3 _targetVelocity;
	[Export]
	public Vector3 MoveDirection { get; set; }
	private IPhysicsComponent PhysicsComponent { get; set; }
	public IActor Actor { get; set; }
	
	public void PhysicsTick(float delta)
	{
		MoveCharacter(delta);
	}


	private void MoveCharacter(float delta)
	{
		
		var moveDirection = MoveDirection.Normalized();
		var unitVel = _targetVelocity.Normalized();
		float velocityDotProduct = unitVel.Dot(PhysicsComponent.Velocity);
		float acceleration = Acceleration * AccelerationCurve.Sample(velocityDotProduct);
		var goalVelocity = moveDirection * MaxSpeed;
		_targetVelocity = _targetVelocity.MoveToward(goalVelocity, acceleration * delta);
		
		
		var maxAcceleration = MaxAccelerationForce * AccelerationCurve.Sample(velocityDotProduct);
		var neededAcceleration = ((_targetVelocity - PhysicsComponent.Velocity) / delta).LimitLength(maxAcceleration);
		
		PhysicsComponent.ApplyForce(neededAcceleration);
		
	}
	public void ProcessTick(float delta)
	{
	}

	public void OnMessage(ActorMessage message)
	{
	}

	public void Setup()
	{
		
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent>();
		GD.Print(PhysicsComponent.GetType());
	}

	public void MoveInDirection(Vector3 direction)
	{
		MoveDirection = direction; 
	}
}
