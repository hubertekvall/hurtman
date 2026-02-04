using Godot;

namespace Hurtman.Actor.Components;
[GlobalClass, Tool]
public partial class ActorRigidBody : RigidBody3D
{
	[Signal] 
	public delegate void CollisionOccurredEventHandler(CollisionObject3D collider, Vector3 position, Vector3 normal, Vector3 relativeVelocity);

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		// Keep default physics behavior
		base._IntegrateForces(state);

		// Just handle the collision data extraction
		int contacts = state.GetContactCount();
		for (int i = 0; i < contacts; i++)
		{
			var collider = InstanceFromId(state.GetContactColliderId(i)) as CollisionObject3D;
			if (collider == null) continue;

			EmitSignal(SignalName.CollisionOccurred, 
				collider, 
				state.GetContactColliderPosition(i), 
				state.GetContactLocalNormal(i), 
				state.GetLinearVelocity() - state.GetContactColliderVelocityAtPosition(i)
			);
		}
	}
}
