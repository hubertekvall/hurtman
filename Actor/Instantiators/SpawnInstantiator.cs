namespace Hurtman.Actor;

public partial class SpawnInstantiator : Instantiator
{
    protected override void Setup()
    {
        Instantiate(Actor.Position, Actor.Basis);
    }
}