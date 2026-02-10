using Godot;

namespace Hurtman.Actor.Components;



public interface IMovement3D
{
	public void MoveInDirection(Vector3 direction);
}