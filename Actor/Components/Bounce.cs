using Godot;
namespace Hurtman.Actor;

[GlobalClass]
public partial class Bounce : ActorComponent
{
	[Export]
	public CharacterComponent Character { get; set; }
	
	public override void OnMessage(ActorMessage message)
	{
		if (message is not CollisionMessage collisionMessage) return;

	
		Character.Body3D.Velocity = Character.PreviousVelocity.Bounce(collisionMessage.Normal) * 1.0f;
	
	}
}
