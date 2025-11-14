namespace Hurtman.Actor;


public partial class DamageMessage(double damage) : ActorMessage
{
    public double Damage { get; set; } = damage;
}