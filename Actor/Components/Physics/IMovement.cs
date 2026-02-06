using Godot;

namespace Hurtman.Actor.Components;



public interface IMovement3D
{
	public Vector3 MoveInDirection(Vector3 direction);
}