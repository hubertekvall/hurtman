namespace Hurtman.Actor;

public interface IActorComponent
{
    public IActor Actor {get; set;}
    public void PhysicsTick(float delta);
    public void ProcessTick(float delta);
    public void OnMessage(ActorMessage message);
    public void Setup();

}