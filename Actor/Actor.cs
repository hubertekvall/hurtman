#nullable enable
using System;
using Godot.Collections;
using System.Collections.Generic;
using Godot;

namespace Hurtman.Actor;

public abstract partial class Actor : Node, IActor
{
    public List<IActorComponent> Components { get; } = new List<IActorComponent>();
    
    public override void _Ready()
    {
        GatherComponents();
        SetupComponents();
        CallDeferred(MethodName.PostReady);
    }
    
    public void GatherComponents()
    {
        foreach (Node component in GetChildren())
        {
            if (component is IActorComponent actorComponent)
            {
                Components.Add(actorComponent);
            }
        }
    }

    public void SetupComponents()
    {
        foreach (IActorComponent component in Components)
        {
            component.Setup(this);
        }
    }
    

    public abstract void PostReady();

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) return;

        foreach (var component in Components)
        {
            component.ProcessTick((float)delta);
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;
        foreach (var component in Components)
        {
            component.PhysicsTick((float)delta);
        }
    }


    public void ReceiveMessage(ActorMessage message)
    {
        foreach (IActorComponent component in Components)
        {
            component.OnMessage(message);
        }
    }

    public void Kill(DeathCause cause)
    {
        if (IsQueuedForDeletion()) return;

        BroadCastMessage(new DeathMessage(cause));
        QueueFree();
    }

    public void RegisterComponent(IActorComponent component)
    {
        Components.Add(component);
    }

    public void BroadCastMessage(ActorMessage message)
    {
        SendMessage(message, this);
    }

    public void SendMessage(ActorMessage message, IActor recipient)
    {
        message.Sender = this;
        recipient.ReceiveMessage(message);
    }
}














public static class NodeExtensions
{
    public static T AddNodeInEditor<T>(this Node node) where T : Node, new()
    {
       
        
        var addedNode = new T();
        node.AddChild(addedNode);
        addedNode.Owner = node.GetTree().EditedSceneRoot;

        return addedNode;
    }
}


