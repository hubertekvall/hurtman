using System;
using System.Reflection.Metadata;
using Godot;
using Godot.Collections;

namespace Hurtman.Actor;

[GlobalClass]

[Tool]
public partial class CharacterComponent : ActorComponent    
{
	
	[Export] public Vector3 Gravity { get; set; }
	
	[Export] public Vector3 LocalImpulse{get; set;}
	[Export] public Vector3 LocalForce{get; set;}
	[Export] public Vector3 Impulse {get; set;}
	[Export] public Vector3 Force {get; set;}
	
	
	[Export] public float  Drag { get; set; } = 0.95f;
	
	[Export] public CharacterBody3D Body3D { get; set; }
	
	public float Speed { get; set; }
	public Vector3 Acceleration { get; set; }
	public Vector3 PreviousVelocity {get; set;}
	public Vector3 LookVector{get; set;}

	protected override void Setup()
	{
		if (Body3D is null)
		{
			Body3D = new CharacterBody3D();
			AddChild(Body3D);
			Body3D.SetOwner(GetTree().EditedSceneRoot);
		}
		
		Body3D.Position = Actor.Position;
		Body3D.Basis = Actor.Basis;
		var relativeVelocity = Body3D.Basis * (LocalImpulse);
		var initialVelocity = Impulse + relativeVelocity;
		Body3D.Velocity = relativeVelocity + initialVelocity;
	}

	public override void PhysicsTick(float delta)
	{
		AddVelocities(delta);
		LookAt(delta);
		MoveCharacter(delta);
	}

	public override void ProcessTick(float delta) 
	{ 
		Actor.Position = Body3D.Position;
		Actor.Basis = Body3D.Basis;
	}

	public override void OnMessage(ActorMessage message) { }


	public virtual void AddVelocities(float delta)
	{
		PreviousVelocity = Body3D.Velocity;
		var relativeConstantVelocity = Body3D.Basis * LocalForce;	
		Body3D.Velocity += (Acceleration * Speed) * delta;
		Body3D.Velocity += Gravity * delta;
		Body3D.Velocity += relativeConstantVelocity * delta;
		Body3D.Velocity += Force * delta;
		Body3D.Velocity = Body3D.Velocity.Lerp(Vector3.Zero, Drag * delta);
	}

	public virtual void MoveCharacter(float delta)
	{
		if (!Body3D.MoveAndSlide()) return;
	
		for (int c = 0; c < Body3D.GetSlideCollisionCount(); c++)
		{
			
			
			var collision = Body3D.GetSlideCollision(c);
			
			HandleCollision(collision);
		}
	}

	public virtual void HandleCollision(KinematicCollision3D collision)
	{
		if (collision.GetCollider() is not CollisionObject3D collisionObject) return;
	
		if (collisionObject.GetParent() is CharacterComponent characterComponent)
		{
			
			var collisionMessage = new CollisionMessage(characterComponent.Actor, collision.GetPosition(), collision.GetNormal());
			Actor.SendMessage(collisionMessage, Actor);
		}
	}
	
	
	public virtual void LookAt(float delta)
	{
		var normalizedLook = LookVector.Normalized();
		var forwardVector = -normalizedLook;

		// Choose a suitable up reference depending on direction
		Vector3 refUp = (Mathf.Abs(forwardVector.Dot(Vector3.Up)) > 0.999f)
			? Vector3.Forward 
			: Vector3.Up;

		// Construct orthogonal basis
		var rightVector = refUp.Cross(forwardVector);
		var upVector = forwardVector.Cross(rightVector);

		// Only build transform if cross product is valid (non-degenerate)
		if (!refUp.Cross(forwardVector).IsZeroApprox())
		{
			var newBasis = new Basis(rightVector.Normalized(), upVector.Normalized(), forwardVector.Normalized());
			var xForm = new Transform3D(newBasis.Orthonormalized(), Body3D.Position);

			Body3D.GlobalTransform = Body3D.GlobalTransform.InterpolateWith(xForm, 2.0f * delta);
		}

		// Update LookVector based on movement (if that's your design)
		LookVector = Body3D.Velocity.Normalized();

	}
}
