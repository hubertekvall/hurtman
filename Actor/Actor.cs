using Godot;
using System;
using Godot.Collections;


namespace Hurtman.Actor;
[GlobalClass]
[Tool]
public partial class Actor : Node3D
{


	
	[Signal]
	public delegate void OnCreationEventHandler();

	[Signal]
	public delegate void OnDeathEventHandler();
	
	[Signal]
	public delegate void OnMessageEventHandler(ActorMessage message);
	
	[Signal]
	public delegate void OnMessageSentEventHandler(ActorMessage message);

	
	public Array<Node> Components { get; set; } = new Array<Node>();
	
	public override void _Ready()
	{
		foreach (Node component in GetChildren())
		{
			if(component is ActorComponent actorComponent){
				Components.Add(actorComponent);
			}
		}

		
		CallDeferred("_PostReady");
		EmitSignalOnCreation();
	}

	public virtual void _PostReady() {}
	

	
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()) return;
		
		foreach (var component in Components)
		{
			if (component is ActorComponent actorComponent)
			{
				actorComponent.ProcessTick((float)delta);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;
		
		foreach (var component in Components)
		{
			if (component is ActorComponent actorComponent)
			{
				actorComponent.PhysicsTick((float)delta);
			}
		}
		
	}
	
	
	public void ReceiveMessage(ActorMessage message)
	{
		
		EmitSignalOnMessage(message);
	}


	public void Kill()
	{
		if (IsQueuedForDeletion()) return;
		EmitSignalOnDeath();
		
		QueueFree();
	}
	
	
	public void RegisterComponent(ActorComponent component){
		Components.Add(component);
	}




	public void SendMessage(ActorMessage message, Actor recipient)
	{
	
		message.Sender = this;
		EmitSignalOnMessageSent(message);
		recipient.ReceiveMessage(message);
	}
}
