using Godot;

namespace Hurtman.Actor;

[GlobalClass]
public partial class LifetimeComponent : Node, IActorComponent
{
    
    [Export]
    public double Lifetime { get; set; }


    public IActor Actor { get; set; }
 


    public  void Setup()
    {
        GetTree().CreateTimer(Lifetime).Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(() => Actor.Kill(DeathCause.Timeout)));
    }
}