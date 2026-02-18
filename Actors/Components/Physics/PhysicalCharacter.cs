using Godot;
using Hurtman.Actors.Components.Physics;

namespace Hurtman.Actors.Components;

[GlobalClass, Tool]
public partial class PhysicalCharacter : SpringCharacter, IMovement3D
{


	[Export] public float MaxSpeed { get; set; }
	[Export] public float Acceleration { get; set; }
	[Export] public float MaxAccelerationForce { get; set; }
	[Export] public Curve AccelerationCurve { get; set; }

	private Vector3 _targetVelocity;
	
	public Vector3 MoveDirection { get; set; }

	public Actor Actor { get; set; }

	public override void PhysicsTick(float delta)
	{
		base.PhysicsTick(delta);
		MoveCharacter(delta);
	}

	private void MoveCharacter(float delta)
	{
		if(PhysicsComponent3D == null) return;
		var moveDirection = MoveDirection.Normalized();
		var unitVel = _targetVelocity.Normalized();
		float velocityDotProduct = unitVel.Dot(PhysicsComponent3D.Velocity.Normalized());
		float acceleration = Acceleration * AccelerationCurve.Sample(velocityDotProduct);
		var goalVelocity = moveDirection * MaxSpeed;
		_targetVelocity = _targetVelocity.MoveToward(goalVelocity, acceleration * delta);

		var currentHorizontalVel = new Vector3(PhysicsComponent3D.Velocity.X, 0, PhysicsComponent3D.Velocity.Z);
		var maxAcceleration = MaxAccelerationForce * AccelerationCurve.Sample(velocityDotProduct);
		var neededAcceleration = ((_targetVelocity - currentHorizontalVel) / delta).LimitLength(maxAcceleration);

		neededAcceleration = Results?.Count > 0 ? neededAcceleration : neededAcceleration * 0.1f;
		
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
