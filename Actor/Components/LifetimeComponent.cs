using Godot;

namespace Hurtman.Actor;

[GlobalClass]
public partial class LifetimeComponent : ActorComponent
{
    
    [Export]
    public double Lifetime { get; set; }


    protected override void Setup()
    {
        GetTree().CreateTimer(Lifetime).Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(() => Actor.Kill(DeathCause.Timeout)));
    }
}