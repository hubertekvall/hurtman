using Godot;

namespace Hurtman.Actor.Components;

public abstract partial class MovementComponent : ActorComponent
{
	[Export]
	public Vector3 Gravity { get; set; } = ProjectSettings.GetSetting("physics/3d/default_gravity_vector").AsVector3() *
										   (float)ProjectSettings.GetSetting("physics/3d/default_gravity").AsDouble();

	[Export] public Vector3 LocalForce { get; set; }
	[Export] public Vector3 Force { get; set; }
	[Export] public float Drag { get; set; } = 0.95f;

	public float Speed { get; set; }
	public Vector3 Acceleration { get; set; }
	public Vector3 LookVector { get; set; }
	public Vector3 Velocity { get; protected set; }

	protected abstract CollisionObject3D Body3D { get; }

	protected override void Setup()
	{
		SetupBody();
		// Ensure the body starts where the Actor is
		if (Body3D != null) Body3D.GlobalTransform = Actor.GlobalTransform;
	}

	protected abstract void SetupBody();
	public abstract Vector3 GetVelocity();
	public abstract void SetVelocity(Vector3 velocity);

	public override void PhysicsTick(float delta)
	{
		ApplyMovement(delta);
		ApplyRotation(delta);

		// Mandatory Synchronization: The Actor follows the Physics Body
		if (Body3D != null && Actor != null)
		{
			Actor.GlobalTransform = Body3D.GlobalTransform;
		}
	}

	protected abstract void ApplyMovement(float delta);

	protected abstract void ApplyRotation(float delta);

	protected void HandleCollisionData(CollisionObject3D collider, Vector3 position, Vector3 normal, Vector3 relVel)
	{
		var msg = collider.GetParent() is Actor other
			? new ActorCollisionMessage(other, collider, position, normal, relVel)
			: new CollisionMessage(collider, position, normal, relVel);

		Actor.SendMessage(msg, Actor);
	}
}
