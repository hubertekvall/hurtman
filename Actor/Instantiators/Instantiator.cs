using System;
using Godot;
using Godot.Collections;
namespace Hurtman.Actor;

[GlobalClass]
public  partial class Instantiator : Node3D
{
	[Export]
	public PackedScene ActorScene { get; set; }
	
	[Export]
	public bool Local  { get; set; }

	public void InstantiateWithMessage(ActorMessage message)
	{
		if(message is CollisionMessage collisionMessage)
		{
			Instantiate(collisionMessage.CollisionPosition, collisionMessage.Sender.Basis);
		}

	}


	public Actor? Instantiate(Vector3 position, Basis basis)
	{
	
		var actor = Instantiate();
	

		actor.Position = position;
		actor.Basis = basis;

		
		return actor;
	}
	
	public Actor? Instantiate()
	{
		
		if(ActorScene is null) return null;
		
		var instance = ActorScene.Instantiate();
		if (instance is not Actor actor) throw new Exception("Scene root must be of the Actor class");
		
		foreach(var component in GetChildren().Duplicate()){
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

	
	

}
