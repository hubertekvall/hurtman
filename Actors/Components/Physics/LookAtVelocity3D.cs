using System;
using Godot;

namespace Hurtman.Actors.Components.Physics;

public partial class LookAtVelocity3D : LookAt3D
{
	public Actor Actor { get; set; }
	private IPhysicsComponent3D PhysicsComponent { get; set; }
	public new void Setup()
	{
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent3D>();
	}



	protected override void LookTransform()
	{
		if (PhysicsComponent == null) return;
		
		Vector3 velocity = PhysicsComponent.Velocity;
		if (velocity.LengthSquared() < 0.01f) return;

		LookVector = velocity;
		
		base.LookTransform();

	}
}
