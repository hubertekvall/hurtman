using Godot;
namespace Hurtman.Actor.Instantiators;

[GlobalClass, Tool] 
public partial class CollisionInstantiator : Instantiator, IMessageHandler
{
	[Export] public float VelocityTreshold { get; set; } = 10.0f;
	
	public void OnMessage(ActorMessage message)
	{
		if (message is not CollisionMessage collisionMessage) return;
		if (collisionMessage.RelativeVelocity.Length() < VelocityTreshold) return;
		
		
		Instantiate(collisionMessage.CollisionPosition, new Basis());
	}
}
