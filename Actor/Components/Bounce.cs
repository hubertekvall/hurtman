using Godot;
namespace Hurtman.Actor;

[GlobalClass]
public partial class Bounce : ActorComponent
{
	[Export]
	public Components.MovementComponent MovementComponent { get; set; }
	
	
	[Export]
	public float BounceFactor = 1.0f;

	public override void OnMessage(ActorMessage message)
	{
	
		if (message is not CollisionMessage collisionMessage) return;
			
			MovementComponent.SetVelocity(MovementComponent.GetVelocity().Bounce(collisionMessage.Normal) * BounceFactor);
	
	}
}
