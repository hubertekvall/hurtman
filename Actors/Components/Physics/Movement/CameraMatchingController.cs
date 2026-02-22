using Godot;
using System;

namespace Hurtman.Actors.Components.Physics;


[GlobalClass, Tool]
public partial class CameraMatchingController : Node, IActorComponent, IPhysicsHandler
{
	
	private IMovement3D MovementComponent {get; set;}
	private Camera3D CameraComponent {get; set;}
	public Actor Actor { get; set; }

	public void PhysicsTick(float delta)
	{

		
		if (MovementComponent == null || CameraComponent == null) return;

		var inputVector = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		var inputVector3 = new Vector3(inputVector.X, 0, inputVector.Y);
		var inputTransformed = (CameraComponent.GlobalBasis * inputVector3) * new Vector3(1, 0 , 1);
		
		MovementComponent.MoveInDirection(inputTransformed);
	}



	public void Setup()
	{
		
		CameraComponent = GetViewport().GetCamera3D();
		MovementComponent = Actor.GetComponent<IMovement3D>();
		
	}
}
