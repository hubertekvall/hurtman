using Godot;

namespace Hurtman.Actors.Components.Physics;

public partial class SpringAnchor : IActorComponent, IPhysicsHandler
{
    public IPhysicsComponent3D PhysicsComponent { get; set; }
    public Actor Actor { get; set; }

    [Export]
    public Vector3 AnchorPoint { get; set; } = Vector3.Zero;

    /// <summary>How strongly the spring pulls toward the anchor. Higher = stiffer.</summary>
    public float SpringStrength { get; set; } = 50f;

    /// <summary>How much the spring resists oscillation. At 1.0 it is critically damped (no overshoot).</summary>
    public float DampingRatio { get; set; } = 0.8f;

    public void Setup()
    {
        PhysicsComponent = Actor.GetComponent<IPhysicsComponent3D>();
    }

    public void PhysicsTick(float delta)
    {
        Vector3 displacement = PhysicsComponent.GlobalTransform.Origin - AnchorPoint;

        // Spring force: F = -k * x
        Vector3 springForce = -SpringStrength * displacement;

        // Damping force: F = -c * v,  where c = 2 * dampingRatio * sqrt(k)
        float dampingCoefficient = 2f * DampingRatio * Mathf.Sqrt(SpringStrength);
        Vector3 dampingForce = -dampingCoefficient * PhysicsComponent.Velocity;

        PhysicsComponent.ApplyForce(springForce + dampingForce);
    }
}