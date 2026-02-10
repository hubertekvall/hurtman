using System;
using Godot;
using Godot.Collections;
namespace Hurtman.Actor;

[GlobalClass]
public partial class Instantiator3D : Node3D, IActorComponent
{
	[Export]
	public PackedScene ActorScene { get; set; }
	
	[Export]
	public bool Local  { get; set; }





	public Actor Instantiate(Vector3 position, Basis basis)
	{
		var actor = Instantiate();
		Position = position;
		Basis = basis;
		return actor;
	}
	
	public Actor Instantiate()
	{
		
		if(ActorScene is null) return null;
		
		var instance = ActorScene.Instantiate();
		if (instance is not Actor actor) throw new Exception("Scene root must be of the IActor interface");
		
		foreach(var component in GetChildren().Duplicate())
		{
			actor.AddChild(component);
		}
		
		if (Local)
		{
			AddChild(actor);
		}
		else
		{
			GetViewport().AddChild(actor);
		}
		
		return actor;
	}


	public IActor Actor { get; set; }
	public void PhysicsTick(float delta)
	{
	}

	public void ProcessTick(float delta)
	{
	}

	public void OnMessage(ActorMessage message)
	{
	}

	public void OnInput(InputEvent inputEvent)
	{

	}

	public void Setup()
	{
	}
}
