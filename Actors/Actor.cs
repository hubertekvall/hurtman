#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using Godot;
using Hurtman.Actors.Components;
using Hurtman.Utilities.Pooling;

namespace Hurtman.Actors;

[GlobalClass, Tool]
public partial class Actor : Node, IPoolable<Actor>
{
	private Dictionary<Type, IActorComponent> Components { get; } = new();
	public ActorMessage InitMessage { get; set; } 
	
	
	public override void _Ready()
	{
		GatherComponents();
		SetupComponents();
		CallDeferred(MethodName.PostReady);
	}

	private void GatherComponents()
	{
		var callableComponents = FindChildren("*");

		foreach (Node component in callableComponents)
		{
			if (component is IActorComponent actorComponent)
			{
				Components.Add(actorComponent.GetType(), actorComponent);
				if (actorComponent is IMessageHandler messageHandler)
				{
					messageHandler.OnMessage(InitMessage);
				}
				
			}
		}
	}

	private void SetupComponents()
	{
		foreach (IActorComponent component in Components.Values)
		{
			component.Actor = this;
			component.Setup();
		}
	}




	protected virtual void PostReady()
	{
	}

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


	public override void _UnhandledInput(InputEvent @event)
	{
		if (Engine.IsEditorHint()) return;
		
		foreach (var component in Components.Values)
		{
			if (component is IInputHandler inputHandler)
			{
				inputHandler.OnInput(@event);
			}
		}
	}
	


	private void ReceiveMessage(ActorMessage message)
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
		
		//if (Pool.Return(this))
		//{
			//GetParent().RemoveChild(this);
			//return;
		//}
		//
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

	public void SendMessage(ActorMessage message, Actor recipient)
	{
		message.Sender = this;

		recipient.ReceiveMessage(message);
	}

	public T? GetComponent<T>() where T : class
	{
		return Components.Values.OfType<T>().FirstOrDefault();
	}


	public IEnumerable<T>? GetAllComponent<T>() where T : class
	{
		return Components.Values.OfType<T>();
	}

	public ObjectPool<Actor> Pool { get; set; }
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
