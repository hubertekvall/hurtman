using Godot;
namespace Hurtman.Actor;

[GlobalClass]
public partial class Bounce : ActorComponent
{
	[Export]
	public CharacterComponent Character { get; set; }
	
	
	[Export]
	public float BounceFactor = 1.0f;

	protected override void OnMessage(ActorMessage message)
	{
	
		if (message is not CollisionMessage collisionMessage) return;

		
		Character.Body3D.Velocity = Character.PreviousVelocity.Bounce(collisionMessage.Normal) * BounceFactor;
	
	}
}
