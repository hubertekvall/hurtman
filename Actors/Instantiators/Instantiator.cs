using System;
using Godot;
using Hurtman.Utilities.Pooling;

namespace Hurtman.Actors.Instantiators;




public partial class Instantiator : Node, IActorComponent
{
	[Export]
	public PackedScene ActorScene { get; set; }
	
	[Export]
	public bool Local  { get; set; }

	
	[Export]
	public bool Pooled { get; set; }
	
	[Export]
	public String PoolName { get; set; }
	
	private ObjectPool<Actor> _actorPool;
	
	
	private Actor Instantiate()
	{
		GD.Print(_actorPool.ActiveObjectCount);
		var actor = Pooled ? _actorPool.Get(InstantiateActor) : InstantiateActor();
		actor.Pool = _actorPool;
		
		return actor;
	}

	private Actor InstantiateActor()
	{
			
		if(ActorScene is null) return null;
		
		var instance = ActorScene.Instantiate();
		if (instance is not Actor actor) throw new Exception("Scene root must be of the IActor interface");

		if (Local) AddChild(actor);
		else GetViewport().AddChild(actor);
		
	
		return actor;
	}


	public Actor Instantiate3D(Vector3 position, Basis basis)
	{
		var actor = Instantiate();
		Actor.SendMessage(new TeleportMessage3D(position), actor);

		return actor;
	}

	public Actor Instantiate2D(Vector2 position, Transform2D transform)
	{
		// TBA: Implemented
		
		return Instantiate();
	}

	public Actor Actor { get; set; }
	public void Setup()
	{
		if(Pooled)  _actorPool = ObjectPool<Actor>.GetPool(PoolName);
		
	}
}
