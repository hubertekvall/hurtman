using Godot;

namespace Hurtman.Actor.Components;



public interface IMovement 
{
	public Vector3 Direction { get; set; }	
	public float Force { get; set; }
	
	public void SetVelocity(Vector3 velocity);
	public Vector3 GetVelocity();

	public void ApplyForce();
	public void ApplyGravity();
	public void ApplyDamping();
}