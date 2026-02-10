#nullable enable
using System;

using System.Collections.Generic;
using Godot;

namespace Hurtman.Actor;

[GlobalClass, Tool]
public  partial class Actor : Node, IActor
{
	public Dictionary<Type, IActorComponent> Components { get; } = new();
	
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
				Components.Add(actorComponent.GetType(), actorComponent);
			}
			GD.Print("Gathering Components");
		}
	}

	public void SetupComponents()
	{
		foreach (IActorComponent component in Components.Values)
		{
			component.Actor = this;
			component.Setup();
			GD.Print("Setup Components");
		}
	}
	

	public virtual void PostReady(){}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()) return;

		foreach (var component in Components.Values)
		{
			if (component is IProcessHandler processHandler)
			{
				processHandler.ProcessTick((float)delta);
			}
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;
		foreach (var component in Components.Values)
		{
			if (component is IPhysicsHandler physicsHandler)
			{
				physicsHandler.PhysicsTick((float)delta);
			}
			
		}
	}


	public void ReceiveMessage(ActorMessage message)
	{
		foreach (var component in Components.Values)
		{
			if (component is IMessageHandler messageHandler)
			{
				messageHandler.OnMessage(message);
			}
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
		// component.Actor = this;
		// Components.Add(component);
		// component.Setup();
	}

	public void AddComponent(IActorComponent component)
	{
		// Components.Add(component);
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
