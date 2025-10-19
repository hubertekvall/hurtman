using Godot;
namespace Hurtman.Actor;


public abstract partial class ActorComponent : Node
{
    [Export]
    public Actor Actor { get; set; }

    public abstract void PhysicsTick(double delta);
    public abstract void ProcessTick(double delta);

   
}
