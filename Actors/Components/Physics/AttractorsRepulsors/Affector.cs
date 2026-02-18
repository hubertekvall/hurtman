using Godot;

namespace Hurtman.Actors.Components.Physics.AttractorsRepulsors;

[Tool]
public partial class Affector : ShapeCast3D, IActorComponent, IPhysicsHandler
{
    public Actor Actor { get; set; }

    private float _radius;

    [Export] public float Force { get; set; }

    [Export(PropertyHint.Range, "2.0, 10.0")]
    public float DepthMultiplier { get; set; }

    [Export]
    public float Radius
    {
        get => _radius;
        set => UpdateRadius(value);
    }

    [Export] public bool Continuous { get; set; } = false;

    private SphereShape3D _sphere;

    public void Setup()
    {
        Enabled = false;
    }

    private void UpdateRadius(float radius)
    {
        _radius = radius;
        _sphere ??= new SphereShape3D();
        _sphere.Radius = radius;
        Shape = _sphere;
    }

    public void PhysicsTick(float delta)
    {
        ForceShapecastUpdate();
        if (!IsColliding()) return;

        for (var i = 0; i < GetCollisionCount(); i++)
        {
            ProcessCollision(i);
        }

        if (!Continuous) OnAfterImpulse();
    }

    protected virtual void ProcessCollision(int collisionIndex)
    {
        var collider = GetCollider(collisionIndex);

        if (collider is not IPhysicsComponent3D physicsComponent) return;

        var colliderNode = collider as Node3D;
        var direction = GetDirectionToCollider(colliderNode);
        var distance = GetDistanceToCollider(colliderNode);
        var falloff = CalculateFalloff(distance);
        var force = CalculateForceVector(direction, distance, physicsComponent.Velocity, falloff);

        
        ApplyForceToComponent(physicsComponent, force);
    }

    protected virtual Vector3 GetDirectionToCollider(Node3D colliderNode)
    {
        return GlobalPosition.DirectionTo(colliderNode.GlobalPosition);
    }

    protected virtual float GetDistanceToCollider(Node3D colliderNode)
    {
        return GlobalPosition.DistanceTo(colliderNode.GlobalPosition);
    }

    // Radius falloff: 1.0 at center, 0.0 at the edge of the radius
    protected virtual float CalculateFalloff(float distance)
    {
        if (_radius <= 0f) return 1f;
        return 1f - Mathf.Clamp(distance / _radius, 0f, 1f);
    }

    protected virtual Vector3 CalculateForceVector(Vector3 direction, float distance, Vector3 velocity, float falloff)
    {
        return direction * Force * falloff;
    }

    protected virtual void ApplyForceToComponent(IPhysicsComponent3D physicsComponent, Vector3 force)
    {
        if (Continuous) physicsComponent.ApplyForce(force);
        else physicsComponent.ApplyImpulse(force);
    }

    protected virtual void OnAfterImpulse()
    {
        Actor.Kill(DeathCause.Optimization);
    }
}