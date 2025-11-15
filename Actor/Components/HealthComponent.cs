using Godot;

namespace Hurtman.Actor;
[GlobalClass]
[Tool]
public partial class HealthComponent : ActorComponent
{
	[Export]
	public double MaxHealth { get; set; }

	public double CurrentHealth { get; set; }


	protected override void Setup()
	{
		CurrentHealth = MaxHealth;
	}

	protected override void OnMessage(ActorMessage message)
	{
		if (message is not DamageMessage damageMessage) return;
		if (CurrentHealth <= 0.0) return;
		
		
		CurrentHealth -= damageMessage.Damage;
   
		if (CurrentHealth <= 0.0) Actor.Kill(DeathCause.Damage);
	}

}
