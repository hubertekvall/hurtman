using Godot;

namespace Hurtman.Actors.Components;



public interface IMovement3D : IPhysicsHandler, IProcessHandler
{
	public void MoveInDirection(Vector3 direction);
	public IPhysicsComponent3D PhysicsComponent3D { get; set; }
}