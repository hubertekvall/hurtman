using Godot;

namespace Hurtman.Actor.Components;


[GlobalClass, Tool]
public partial class CharacterMovementComponent : MovementComponent
{
	[Export] public CharacterBody3D CharacterBody;
	protected override CollisionObject3D Body3D => CharacterBody;

	protected override void SetupBody()
	{
		if (CharacterBody == null) 
		{
			GD.PrintErr($"{nameof(CharacterMovementComponent)} on {Actor.Name} is missing a CharacterBody3D reference!");
		}
	}

	public override Vector3 GetVelocity() => CharacterBody.Velocity;
	public override void SetVelocity(Vector3 velocity) => CharacterBody.Velocity = velocity;

	protected override void ApplyMovement(float delta)
	{
		
		
		if (CharacterBody.MoveAndSlide())
		{
			for (int i = 0; i < CharacterBody.GetSlideCollisionCount(); i++)
			{
				var col = CharacterBody.GetSlideCollision(i);
				HandleCollisionData(col.GetCollider() as CollisionObject3D, col.GetPosition(), col.GetNormal(), Velocity - col.GetColliderVelocity());
			}
		}
	}

	protected override void ApplyRotation(float delta)
	{
		var targetBasis = Basis.LookingAt(-LookVector.Normalized(), Vector3.Up);
		var currentBasis = Body3D.Transform.Basis.Orthonormalized();
		var newBasis = currentBasis.Slerp(targetBasis, 10.0f * delta);


		if (!Velocity.IsZeroApprox())
		{
			LookVector = Velocity.Normalized();
		} 
		Body3D.Transform = new Transform3D(newBasis, Body3D.Transform.Origin);
	}
}
