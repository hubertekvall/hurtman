using Godot;

namespace Hurtman.Actor;
[GlobalClass]
[Tool]
public partial class HealthComponent : Node, IActorComponent
{
	[Export]
	public double MaxHealth { get; set; }

	public double CurrentHealth { get; set; }


	public void Setup()
	{
		CurrentHealth = MaxHealth;
	}

	public IActor Actor { get; set; }

	public void PhysicsTick(float delta)
	{
		throw new System.NotImplementedException();
	}

	public void ProcessTick(float delta)
	{
		throw new System.NotImplementedException();
	}

	public  void OnMessage(ActorMessage message)
	{
		if (message is not DamageMessage damageMessage) return;
		if (CurrentHealth <= 0.0) return;
		
		
		CurrentHealth -= damageMessage.Damage;
   
		if (CurrentHealth <= 0.0) Actor.Kill(DeathCause.Damage);
	}

}
