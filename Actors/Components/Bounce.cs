using Godot;
using Hurtman.Actors.Components;

namespace Hurtman.Actors;

[GlobalClass]
public partial class Bounce : Node, IActorComponent, IMessageHandler
{
	
	private IPhysicsComponent PhysicsComponent{ get; set; }
	
	
	[Export]
	public float BounceFactor = 1.0f;

	public Actor Actor { get; set; }
	

	public  void OnMessage(ActorMessage message)
	{
		if (message is not CollisionMessage collisionMessage) return;
		
		PhysicsComponent.Velocity = PhysicsComponent.Velocity.Bounce(collisionMessage.Normal) * BounceFactor;
	}



	public void Setup()
	{
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent>();
	}
}
