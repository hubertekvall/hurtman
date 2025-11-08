using System;
using Godot;
using Godot.Collections;
namespace Hurtman.Actor;

public abstract partial class Instantiator : Node3D
{
	[Export]
	public PackedScene ActorScene { get; set; }
	
	[Export]
	public bool Local  { get; set; }




	public virtual Actor InstantiateWithMessage(ActorMessage message)
	{
		return message switch
		{
			CollisionMessage collisionMessage => InstantiateWithTransform(collisionMessage.CollisionPosition, collisionMessage.Owner.GlobalBasis),
			_ => Instantiate()
		};
	}
	

	public Actor InstantiateWithTransform(Vector3 position, Basis basis)
	{
		var actor = Instantiate();

		actor.Position = position;
		actor.Basis = basis;

		return actor;
	}
	
	public Actor Instantiate()
	{
		var instance = ActorScene.Instantiate();
		if (instance is not Actor actor) throw new Exception("Scene root must be of the Actor class");
		
		actor.Components = GetChildren().Duplicate();
		
		if (Local)
		{
			AddChild(actor);
		}
		else
		{
			actor.Position = GlobalPosition;
			actor.Basis = GlobalBasis;
			GetViewport().AddChild(actor);
		}
		
		return actor;
	}



}
