namespace Hurtman.Actors;


public partial class DamageMessage(double damage, CollisionMessage collisionMessage) : ActorMessage
{
    public double Damage { get; set; } = damage;
    public CollisionMessage CollisionMessage { get; set; } = collisionMessage;
}