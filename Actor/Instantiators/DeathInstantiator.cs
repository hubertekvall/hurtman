namespace Hurtman.Actor;

public partial class DeathInstantiator : Instantiator
{
    protected override void OnMessage(ActorMessage message)
    {
        if (message is not DeathMessage deathMessage) return;


        Instantiate(Actor.Position, Actor.Basis);
    }
}