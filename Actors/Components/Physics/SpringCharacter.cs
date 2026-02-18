using System.Diagnostics;
using Godot;
namespace Hurtman.Actors.Components.Physics;
[GlobalClass, Tool]
public partial class SpringCharacter : Node, IActorComponent, IPhysicsHandler
{
	[Export] public float RideHeight { get; set; } = 2.0f;
	[Export] public float RideSpringStrength { get; set; } = 50.0f;
	[Export] public float RideSpringDamper { get; set; } = 5.0f;
	[Export] public float RideCastFactor { get; set; } = 1.0f;
	[Export] public float LookAheadDistance { get; set; } = 2.0f;
	[Export] public float CastRadius { get; set; } = 0.5f;
	[Export]
	public bool Debug
	{
		get => _debugNode != null;
		set => SetDebug(value);
	}
	private Control? _debugNode;
	private Vector3 _rayOrigin;
	public IPhysicsComponent3D PhysicsComponent3D { get; set; }
	private void SetDebug(bool value)
	{
		if (!value)
		{
			_debugNode?.QueueFree();
			return;
		}
		_debugNode = _debugNode ?? new Control();
		AddChild(_debugNode);
		_debugNode.Connect(CanvasItem.SignalName.Draw, Callable.From(DrawDebug));
	}
	private void DrawDebug()
	{
		var camera = GetViewport().GetCamera3D();
		var origin = camera?.UnprojectPosition(_rayOrigin) ?? Vector2.Zero;
		var end = camera?.UnprojectPosition(_rayOrigin + Vector3.Down * (RideHeight * RideCastFactor)) ?? Vector2.Zero;
		_debugNode?.DrawLine(origin, end, Colors.Red, 2.0f, true);
	}
	private void SpringFloat(float delta)
	{
		var speed = (PhysicsComponent3D.Velocity * new Vector3(1, 0, 1)).Length();
		var lookAhead = PhysicsComponent3D.Velocity.Normalized() * Mathf.Min(speed * speed, LookAheadDistance);
		_rayOrigin = _rayOrigin.Lerp(PhysicsComponent3D.GlobalTransform.Origin + lookAhead, 10.0f * delta);
		var rayOrigin = _rayOrigin;

		var spaceState = PhysicsComponent3D.GetWorld3D().DirectSpaceState;
		var castEnd = rayOrigin + Vector3.Down * (RideHeight * RideCastFactor);

		// Shape cast using a sphere
		var shape = new SphereShape3D { Radius = CastRadius };
		var query = new PhysicsShapeQueryParameters3D
		{
			Shape = shape,
			Transform = new Transform3D(Basis.Identity, rayOrigin),
			Motion = Vector3.Down * (RideHeight * RideCastFactor),
			CollideWithBodies = true,
			CollideWithAreas = false,
			Exclude = [PhysicsComponent3D.GetRid()]
		};

		var results = spaceState.CastMotion(query);
		// CastMotion returns [safe_fraction, unsafe_fraction] — no hit if safe == 1.0
		if (results[0] >= 1.0f) return;

		// Reconstruct hit distance from the safe fraction
		var castDistance = RideHeight * RideCastFactor;
		var hitDistance = results[0] * castDistance;
		var hitPosition = rayOrigin + Vector3.Down * hitDistance;

		// Get the collider for rigidbody interaction
		var otherVelocity = Vector3.Zero;
		RigidBody3D hitRigidBody = null;

		var contactResults = spaceState.IntersectShape(new PhysicsShapeQueryParameters3D
		{
			Shape = shape,
			Transform = new Transform3D(Basis.Identity, rayOrigin + Vector3.Down * (hitDistance + CastRadius * 0.5f)),
			CollideWithBodies = true,
			CollideWithAreas = false,
			Exclude = [PhysicsComponent3D.GetRid()]
		});

		foreach (var contact in contactResults)
		{
			var collider = contact["collider"].As<Node>();
			if (collider is RigidBody3D rigidBody)
			{
				hitRigidBody = rigidBody;
				otherVelocity = rigidBody.LinearVelocity;
				break;
			}
		}

		var rayDirectionVelocity = Vector3.Down.Dot(PhysicsComponent3D.Velocity);
		var otherDirectionVelocity = Vector3.Down.Dot(otherVelocity);
		var relativeVelocity = rayDirectionVelocity - otherDirectionVelocity;
		var compressionDistance = RideHeight - hitDistance;
		var springForce = (compressionDistance * RideSpringStrength) -
						  (PhysicsComponent3D.Velocity.Y * RideSpringDamper);

		PhysicsComponent3D.ApplyForce(Vector3.Up * springForce);

		if (hitRigidBody != null)
		{
			hitRigidBody.ApplyForce(Vector3.Down * springForce, hitPosition - hitRigidBody.GlobalPosition);
		}
	}
	public Actor Actor { get; set; }
	public void PhysicsTick(float delta)
	{
		SpringFloat(delta);
		_debugNode?.QueueRedraw();
	}
	public void Setup()
	{
		PhysicsComponent3D = Actor.GetComponent<IPhysicsComponent3D>();
	}
}
