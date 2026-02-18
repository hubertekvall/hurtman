using Godot;
namespace Hurtman.Actors.Components;

public interface IPhysicsComponent3D : IPhysicsHandler, IProcessHandler
{

	public World3D GetWorld3D();
	
	public Rid GetRid();
	public Transform3D Transform { get; set; }
	public Transform3D GlobalTransform { get; set; }
	public Vector3 Velocity { get; set; }
	public Vector3 AngularVelocity { get; set; }
	public float Damping { get; set; }
	public void ApplyForce(Vector3 force, Vector3? position = null);
	public void ApplyImpulse(Vector3 impulse, Vector3? position = null);
	public void ApplyTorque(Vector3 torque);
}
