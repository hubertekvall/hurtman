using Godot;
namespace Hurtman.Actors.Components.Physics.Movement;

public partial class AltitudeControl : Node, IActorComponent, IPhysicsHandler
{
    public Actor Actor { get; set; }
    public IPhysicsComponent3D PhysicsComponent { get; set; }
    
    
    [Export]
    public 
    
    
    
    public void Setup()
    {
        PhysicsComponent = Actor.GetComponent<IPhysicsComponent3D>();
    }

    public void PhysicsTick(float delta)
    {
        
    }
}