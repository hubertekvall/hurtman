using Godot;

namespace Hurtman.Actor.Components;

public class PhysicalCharacterController : IActorComponent
{
    
    [Export] public float MaxSpeed { get; set; }
    [Export] public float Acceleration { get; set; }
    [Export] public float MaxAccelerationForce { get; set; }
    [Export] public Curve AccelerationCurve { get; set; }
    
    public Vector3 MoveDirection { get; set; }
    
    public IPhysicsComponent PhysicsComponent { get; set; }
    public IActor Actor { get; set; }
    public void PhysicsTick(float delta)
    {
        var moveDirection = MoveDirection.Normalized();
        float velocityDotProduct = moveDirection.Dot(PhysicsComponent.Velocity);
        float acceleration = Acceleration * AccelerationCurve.Sample(velocityDotProduct);
        var goalVelocity = moveDirection * MaxSpeed;
        goalVelocity = goalVelocity.MoveToward(goalVelocity, acceleration * delta);
        
        PhysicsComponent.ApplyForce();
    }

    public void ProcessTick(float delta)
    {
    }

    public void OnMessage(ActorMessage message)
    {
    }
}