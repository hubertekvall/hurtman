using Godot;

namespace Hurtman.Actor.Components;



[GlobalClass, Tool]
public partial class ActorCharacterBody : CharacterBody3D, IActorComponent
{
	public IActor Actor { get; set; }

	public void PhysicsTick(float delta)
	{
		if (Engine.IsEditorHint() || Actor == null) return;

		// Handle collisions from CharacterBody3D
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider();
            
			CollisionMessage collisionMessage;
            
			if (collider is Node colliderNode && colliderNode.GetParent() is IActor otherActor)
			{
				collisionMessage = new ActorCollisionMessage(
					otherActor,
					collider as CollisionObject3D,
					collision.GetPosition(),
					collision.GetNormal(),
					Velocity
				);
			}
			else
			{
				collisionMessage = new CollisionMessage(
					collider as CollisionObject3D,
					collision.GetPosition(),
					collision.GetNormal(),
					Velocity
				);
			}
            
			Actor.BroadCastMessage(collisionMessage);
		}
	}


	public void ProcessTick(float delta) { }
	public void OnMessage(ActorMessage message) { }
}