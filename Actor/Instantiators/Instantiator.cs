using System;
using Godot;

namespace Hurtman.Actor.Instantiators;


public partial class Instantiator : Node3D, IActorComponent
{
	[Export]
	public PackedScene ActorScene { get; set; }
	
	[Export]
	public bool Local  { get; set; }

	[Export]
	public Node3D Holder { get; set; }


	[Export] public float RateLimit { get; set; } = 0.5f;

	private bool canInstantiate = true;

	
	
	public Actor Instantiate(Vector3 position, Basis basis)
	{
		var actor = Instantiate();
		Actor.SendMessage(new TeleportMessage3D(position), actor);
		return actor;
	}
	
	public Actor Instantiate()
	{
		if (!canInstantiate) return null;
		if(ActorScene is null) return null;
		
		var instance = ActorScene.Instantiate();
		if (instance is not Actor actor) throw new Exception("Scene root must be of the IActor interface");
		
		foreach(var component in GetChildren().Duplicate())
		{
			actor.AddChild(component);
		}
		
		if (Local)
		{
			Holder.AddChild(actor);
		}
		else
		{
			GetViewport().AddChild(actor);
		}

		canInstantiate = false;
		GetTree().CreateTimer(RateLimit).Timeout += ToggleInstantiator;
		
		return actor;
	}


	public IActor Actor { get; set; }



	private void ToggleInstantiator()
	{
		canInstantiate = !canInstantiate;
	}
	

	public void Setup()
	{
		Holder ??= this;
	}
}
