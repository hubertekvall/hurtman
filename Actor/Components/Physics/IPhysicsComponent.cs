using Godot;
namespace Hurtman.Actor.Components;

public interface IPhysicsComponent
{
    public Vector3 Velocity { get; set; }
    public Vector3 Acceleration { get; set; }
    public Vector3 Damping { get; set; }
    
    public void ApplyForce(Vector3 force);
    public void ApplyDamping(float damping);
}