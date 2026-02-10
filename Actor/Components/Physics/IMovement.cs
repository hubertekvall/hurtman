using Godot;

namespace Hurtman.Actor.Components;



public interface IMovement3D : IPhysicsHandler, IProcessHandler
{
	public void MoveInDirection(Vector3 direction);
	public IPhysicsComponent PhysicsComponent { get; set; }
}