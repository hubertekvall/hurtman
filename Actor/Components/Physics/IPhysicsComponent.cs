using Godot;
namespace Hurtman.Actor.Components;

public interface IPhysicsComponent
{
    public Vector3 Velocity { get; set; }
    public float Damping { get; set; }
    public void ApplyForce(Vector3 force);
    
}