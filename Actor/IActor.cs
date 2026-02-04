using Godot;
using System.Collections.Generic;

namespace Hurtman.Actor;

public interface IActor
{
    List<IActorComponent> Components { get; }

    public void GatherComponents();
    public void PostReady();
    public  void ReceiveMessage(ActorMessage message);
    public void Kill(DeathCause cause);
    public void RegisterComponent(IActorComponent component);
    public void BroadCastMessage(ActorMessage message);
    public void SendMessage(ActorMessage message, IActor recipient);
}