using Godot;
using Hurtman.Utilities;
namespace Hurtman.Actor;


[GlobalClass]
public partial class  DamageComponent : Node, IActorComponent, IMessageHandler
{
	[Export] 
	public double Damage { get; set; }

	
	[Export(PropertyHint.Range, "1.0, 10.0")]
	public double Spread { get; set; }


	public IActor Actor { get; set; }
	

	public  void OnMessage(ActorMessage message)
	{

		if (message is not ActorCollisionMessage collisionMessage) return;
		Actor.SendMessage(new DamageMessage(SpreadValue.GetRandomSpread(Damage, Spread)), collisionMessage.OtherActor);
	}
	

	public void Setup()
	{
	}
}
