#nullable enable
using System;
using Godot;
using System.Collections.Generic;
using Hurtman.Actor.Components;
using System.Linq;

namespace Hurtman.Actor;

public interface IActor
{
    Dictionary<Type, IActorComponent> Components { get; }

    public void GatherComponents();
    public void PostReady();
    public  void ReceiveMessage(ActorMessage message);
    public void Kill(DeathCause cause);
    public void RegisterComponent(IActorComponent component);
    public void BroadCastMessage(ActorMessage message);
    public void SendMessage(ActorMessage message, IActor recipient);
    
    T? GetComponent<T>()  where T : class
    {
        return Components.Values.OfType<T>().FirstOrDefault();
    }
    
}