using Godot;
using Hurtman.Utilities;
namespace Hurtman.Actor;


[GlobalClass]
public partial class  DamageComponent : ActorComponent
{
	[Export] 
	public double Damage { get; set; }

	
	[Export(PropertyHint.Range, "1.0, 10.0")]
	public double Spread { get; set; }
	


	public override void OnMessage(ActorMessage message)
	{

		if (message is not CollisionMessage collisionMessage) return;
		
		
		
		Actor.SendMessage(new DamageMessage(SpreadValue.GetRandomSpread(Damage, Spread)), collisionMessage.Other);
	}

}
