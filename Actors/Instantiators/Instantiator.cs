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
	
	
	private Actor Instantiate(ActorMessage withMessage = null)
	{

		var actor = Pooled ? _actorPool.Get(InstantiateActor) : InstantiateActor();
		actor.Pool = _actorPool;
		if(withMessage != null) actor.InitMessage = withMessage;
		AddActorToTree(actor);
		return actor;
	}

	private Actor InstantiateActor()
	{
		if(ActorScene is null) return null;
		
		var instance = ActorScene.Instantiate();
		if (instance is not Actor actor) throw new Exception("Scene root must be of the IActor interface");

		return actor;
	}


	private void AddActorToTree(Actor actor)
	{
		if (Local) AddChild(actor);
		else GetViewport().AddChild(actor);
	}


	public Actor Instantiate3D(Vector3 position, Basis basis) => Instantiate(new TeleportMessage3D(position));



	public Actor Instantiate2D(Vector2 position, Transform2D transform)
	{
		// TODO
		return Instantiate();
	}

	public Actor Actor { get; set; }
	public void Setup()
	{
		if(Pooled)
		{
			_actorPool = ObjectPool<Actor>.GetPool(PoolName);
			_actorPool.Warmup(500, InstantiateActor);
		}
	}

	public override void _Notification(int what)
	{
		if (what == Node.NotificationPredelete)
		{
			_actorPool.Clean();
		}
	}
}
