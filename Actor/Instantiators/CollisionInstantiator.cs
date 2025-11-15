using Godot;

namespace Hurtman.Actor;

public partial class CollisionInstantiator : Instantiator
{
	protected override void OnMessage(ActorMessage message)
	{
		if (message is CollisionMessage collisionMessage)
		{
			Instantiate(collisionMessage.CollisionPosition, Actor.Basis);
		}
	}
}
