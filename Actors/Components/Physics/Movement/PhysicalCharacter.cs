using Godot;
using Hurtman.Actors.Components.Physics;
using Hurtman.Actors.Components.Physics.Casting;

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

	public IPhysicsComponent3D PhysicsComponent3D { get; set; }

	private ActorRaycast ActorRaycast { get; set; }


	public Actor Actor { get; set; }

	public void Setup()
	{
		PhysicsComponent3D = Actor.GetComponent<IPhysicsComponent3D>();
		ActorRaycast = Actor.GetComponent<ActorRaycast>();
	}

	public void PhysicsTick(float delta)
	{
		MoveCharacter(delta);
	}

	private void MoveCharacter(float delta)
	{
		if (PhysicsComponent3D == null) return;
		var moveDirection = MoveDirection.Normalized();
		var unitVel = _targetVelocity.Normalized();
		float velocityDotProduct = unitVel.Dot(PhysicsComponent3D.Velocity.Normalized());
		float acceleration = Acceleration * AccelerationCurve.Sample(velocityDotProduct);
		var goalVelocity = moveDirection * MaxSpeed;
		_targetVelocity = _targetVelocity.MoveToward(goalVelocity, acceleration * delta);

		var currentHorizontalVel = new Vector3(PhysicsComponent3D.Velocity.X, 0, PhysicsComponent3D.Velocity.Z);
		var maxAcceleration = MaxAccelerationForce * AccelerationCurve.Sample(velocityDotProduct);
		var neededAcceleration = ((_targetVelocity - currentHorizontalVel) / delta).LimitLength(maxAcceleration);

		neededAcceleration = ActorRaycast.IsColliding() ? neededAcceleration : neededAcceleration * 0.1f;

		PhysicsComponent3D.ApplyForce(neededAcceleration);
	}


	public void MoveInDirection(Vector3 direction)
	{
		MoveDirection = direction;
	}


	public void ProcessTick(float delta)
	{
	}
}
