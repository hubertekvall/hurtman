namespace Hurtman.Actor;

public partial class DamageInstantiator : Instantiator
{
    protected override void OnMessage(ActorMessage message)
    {
        if (message is not DamageMessage damageMessage) return;




        Instantiate(Actor.Position, Actor.Basis);
    }
}