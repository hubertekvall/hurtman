using System;
using Godot;
using Godot.Collections;

namespace Hurtman.Actor.Components;

[Tool]
public partial class CharacterComponent : ActorComponent    
{
	
	[Export] public Vector3 Gravity { get; set; }
	
	[Export] public Vector3 InitialVelocity {get; set;}

	[Export] public float  Drag { get; set; } = 0.05f;
	
	[Export] public CharacterBody3D Body3D { get; set; }
	
	public float Speed { get; set; }
	public Vector3 Acceleration { get; set; }


	public override void _Ready()
	{
		Body3D.Velocity = InitialVelocity;
	}

	public override void PhysicsTick(float delta)
	{
		Body3D.Velocity += (Acceleration * Speed) * delta;
		Body3D.Velocity += Gravity * delta;
		Body3D.Velocity = Body3D.Velocity.Lerp(Vector3.Zero, Drag * delta);
	}

	public override void ProcessTick(float delta) { }
}
