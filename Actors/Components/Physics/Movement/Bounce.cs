using Godot;
using Hurtman.Actors.Components;

namespace Hurtman.Actors;

[GlobalClass]
public partial class Bounce : Node, IActorComponent, IMessageHandler
{
	
	private IPhysicsComponent3D PhysicsComponent3D{ get; set; }
	
	
	[Export]
	public float BounceFactor = 1.0f;

	public Actor Actor { get; set; }
	

	public  void OnMessage(ActorMessage message)
	{
		if (message is not CollisionMessage collisionMessage) return;
		
		PhysicsComponent3D.Velocity = PhysicsComponent3D.Velocity.Bounce(collisionMessage.Normal) * BounceFactor;
	}



	public void Setup()
	{
		PhysicsComponent3D = Actor.GetComponent<IPhysicsComponent3D>();
	}
}
