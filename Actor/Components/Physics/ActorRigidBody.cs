using Godot;

namespace Hurtman.Actor.Components.Physics;

[GlobalClass]
[Tool]
public partial class ActorRigidBody : RigidBody3D, IActorComponent, IPhysicsComponent
{
	//public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	//{
		//// Keep default physics behavior
		//base._IntegrateForces(state);
//
		//if(Actor == null) return;
		//// Just handle the collision data extraction
		//int contacts = state.GetContactCount();
		//for (int i = 0; i < contacts; i++)
		//{
			//var collider = InstanceFromId(state.GetContactColliderId(i)) as CollisionObject3D;
			//if (collider == null) continue;
//
			//CollisionMessage collisionMessage;
//
			//var position = state.GetContactColliderPosition(i);
			//var normal = state.GetContactLocalNormal(i);
			//var relativeVelocity = state.GetLinearVelocity() - state.GetContactColliderVelocityAtPosition(i);
//
			//if (collider.GetParent() is IActor callableActor)
			//{
				//collisionMessage = new ActorCollisionMessage(callableActor, collider, position, normal, relativeVelocity);
			//}
			//else
			//{
				//GD.Print("Collided bitch");
				//collisionMessage = new CollisionMessage(collider, position, normal, relativeVelocity);   
			//}
//
			//Actor.BroadCastMessage(collisionMessage);
		//}
	//}

	public IActor Actor { get; set; }

	public void PhysicsTick(float delta)
	{
	}

	public void ProcessTick(float delta)
	{
	}

	public void OnMessage(ActorMessage message)
	{
	}

	public void OnInput(InputEvent inputEvent)
	{
		
	}

	public void Setup()
	{
	
	}

	
	public Vector3 Velocity { get => this.LinearVelocity; set => this.LinearVelocity = value; }
	public float Damping { get => this.LinearDamp; set => this.LinearDamp = value; }





	public Vector3 Acceleration { get; set; }

}
