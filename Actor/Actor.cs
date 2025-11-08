using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace Hurtman.Actor;
[Tool]
public partial class Actor : Node3D
{


	
	[Signal]
	public delegate void OnCreationEventHandler();

	[Signal]
	public delegate void OnDeathEventHandler();

	[Signal]
	public delegate void OnCollisionEventHandler(CollisionMessage message);
	
	[Signal]
	public delegate void OnHitEventHandler(DamageMessage message);

	[Signal]
	public delegate void OnMessageEventHandler(ActorMessage message);


	
	public Array<Node> Components { get; set; } = new Array<Node>();
	
	public override void _Ready()
	{
		foreach (Node component in Components)
		{
			AddChild(component);
		}
		
		CallDeferred("_PostReady");
		EmitSignalOnCreation();
	}

	public virtual void _PostReady() {}
	

	
	public override void _Process(double delta)
	{
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
		if (message is DamageMessage damageMessage)
		{   
			EmitSignalOnHit(damageMessage);
		}
		else if (message is CollisionMessage collisionMessage)
		{
			EmitSignalOnCollision(collisionMessage);
		}
	}


	public void Kill()
	{
		if (IsQueuedForDeletion()) return;
		EmitSignalOnDeath();
		
		QueueFree();
	}
}
