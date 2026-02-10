namespace Hurtman.Actor;

public interface IPhysicsHandler
{
    public void PhysicsTick(float delta);
}

public interface IProcessHandler
{
    public void ProcessTick(float delta);
}

public interface IMessageHandler
{
    public void OnMessage(ActorMessage message);
}

public interface IInputHandler
{
    
}