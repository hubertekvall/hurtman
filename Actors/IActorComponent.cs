using Godot;

namespace Hurtman.Actors;

public interface IActorComponent
{
	public Actor Actor {get; set;}
	public void Setup();
}
