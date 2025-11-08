using Godot;
namespace Hurtman.Actor;

[Tool]
public abstract partial class ActorComponent : Node
{
    [Export]
    public Actor Actor { get; set; }

    public abstract void PhysicsTick(float delta);
    public abstract void ProcessTick(float delta);

   
}
