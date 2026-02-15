using Godot;

namespace Hurtman.Actors.Components.Physics.AttractorsRepulsors;

[GlobalClass, Tool]
public partial class RepulsorSphere : ShapeCast3D, IActorComponent, IPhysicsHandler
{
	public Actor Actor { get; set; }

	[Export] public float Force { get; set; }
	[Export(PropertyHint.Range, "0.0, 1.0")] public float DepthMultiplier { get; set; }
	

	public void Setup()
	{
		Enabled = false;
	}

	public void PhysicsTick(float delta)
	{
		ForceShapecastUpdate();
		if (!IsColliding()) return;



		for (int i = 0; i < GetCollisionCount(); i++)
		{
			var collider = GetCollider(i);

			if (collider is IPhysicsComponent3D physicsComponent)
			{

				var colliderNode = collider as Node3D;
				var directionTo = GlobalPosition.DirectionTo(colliderNode.GlobalPosition);
				var distance = GlobalPosition.DistanceTo(colliderNode.GlobalPosition);
				var depthModifier = (GlobalPosition.DistanceTo(colliderNode.GlobalPosition) * DepthMultiplier);
				
				
				physicsComponent.ApplyForce(directionTo * Force * depthModifier);
			}
		}
		
	}
}
