using Godot;
namespace Hurtman.Actors.Components.Physics.Movement;

public partial class AltitudeControl : Node, IActorComponent, IPhysicsHandler
{
    public Actor Actor { get; set; }
    public IPhysicsComponent3D PhysicsComponent { get; set; }
    
    
    [Export]
    public float AltitudeTarget { get; set; }

    [Export]
    public float AltitudeStrength { get; set; }
    
    [Export]
    public float AltitudeDamping { get; set; }



    public void Setup()
    {
        PhysicsComponent = Actor.GetComponent<IPhysicsComponent3D>();
    }

    public void PhysicsTick(float delta)
    {
        if(PhysicsComponent == null) return;
        
        var velocity = PhysicsComponent.Velocity;
        var position = PhysicsComponent.GlobalTransform.Origin;
        
        var offset = position.Y - AltitudeTarget;

        var springForce = (-AltitudeStrength * offset) - (AltitudeDamping * velocity.Y);

        PhysicsComponent.ApplyForce(springForce * Vector3.Up);

    }
}