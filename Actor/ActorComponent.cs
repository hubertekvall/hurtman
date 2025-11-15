using Godot;
namespace Hurtman.Actor;

[Tool]
public abstract partial class ActorComponent : Node
{
    [Export]
    public Actor Actor {get; set;}

    public virtual void PhysicsTick(float delta){}
    public virtual void ProcessTick(float delta){}
    protected virtual void OnMessage(ActorMessage message){}
  
    protected virtual void Setup(){}
    
    public override void _Ready()
    {
        Actor ??= GetParent<Actor>();
        Actor.Connect(Actor.SignalName.OnMessage, Callable.From<ActorMessage>(OnMessage));
        Setup();
    }
    

   
}
