using Godot;

namespace Hurtman.Actors.Components.Physics;



[GlobalClass]
[Tool]
public partial class ActorCharacterBody : CharacterBody3D, IActorComponent, IPhysicsComponent3D
{
	public Actor Actor { get; set; }
	public Vector3 AngularVelocity { get; set; }
	public float Damping { get; set; }
	public void ApplyForce(Vector3 force, Vector3? position = null)
	{
		_velocityBuffer += force;
	}

	public void ApplyTorque(Vector3 torque)
	{
		
	}

	private Vector3 _velocityBuffer = Vector3.Zero;
	
	

	public void PhysicsTick(float delta)
	{
		if (Engine.IsEditorHint() || Actor == null) return;

		Velocity += _velocityBuffer;
		MoveAndSlide();
		
		_velocityBuffer = Vector3.Zero;
		HandleCollisions();
	}


	private void HandleCollisions()
	{
		// Handle collisions from CharacterBody3D
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider();
			
			CollisionMessage collisionMessage;
			
			if (collider is Node colliderNode && colliderNode.GetParent() is Actor otherActor)
			{
				collisionMessage = new ActorCollisionMessage(
					otherActor,
					collider as CollisionObject3D,
					collision.GetPosition(),
					collision.GetNormal(),
					Velocity
				);
			}
			else
			{
				collisionMessage = new CollisionMessage(
					collider as CollisionObject3D,
					collision.GetPosition(),
					collision.GetNormal(),
					Velocity
				);
			}
			
			Actor.BroadCastMessage(collisionMessage);
		}
	}
	

	public void ProcessTick(float delta) { }

	public void OnMessage(ActorMessage message)
	{
		if (message is not TeleportMessage3D teleportMessage) return;
		
		GlobalPosition = teleportMessage.Position;
	}
	public void OnInput(InputEvent inputEvent) { }
	public void Setup() { }
}
