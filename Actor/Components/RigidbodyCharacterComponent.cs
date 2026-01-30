using Godot;
using System.Collections.Generic;

namespace Hurtman.Actor;

[GlobalClass]
[Tool]
public partial class RigidBodyMovementComponent : MovementComponent
{
    [Export] public RigidBody3D RigidBody { get; set; }
    [Export] public bool UseGravity { get; set; } = true;
    [Export] public float AngularDamp { get; set; } = 1.0f;
    [Export] public float LinearDamp { get; set; } = 0.1f;

    protected override CollisionObject3D Body3D => RigidBody;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        
        if (RigidBody == null)
        {
            GD.PushWarning($"{Name}: RigidBody is not assigned!");
            return;
        }

        ConfigurePhysicsBody();
    }

    private void ConfigurePhysicsBody()
    {
        RigidBody.GravityScale = UseGravity ? 1.0f : 0.0f;
        RigidBody.AngularDamp = AngularDamp;
        RigidBody.LinearDamp = LinearDamp;
        RigidBody.ContactMonitor = true;
        RigidBody.MaxContactsReported = 4;

        // Register the sync callback for custom physics integration
        var rid = RigidBody.GetRid();
        PhysicsServer3D.BodySetStateSyncCallback(rid, Callable.From<PhysicsDirectBodyState3D>(HandleBodyState));
    }

    public override void SetupBody()
    {
        if (RigidBody == null) return;

        RigidBody.GlobalTransform = Actor.GlobalTransform;
        InitializeVelocities();
    }

    public override void InitializeVelocities()
    {
        if (RigidBody == null) return;

        Vector3 relativeVelocity = RigidBody.Basis * LocalImpulse;
        RigidBody.LinearVelocity = Impulse + relativeVelocity;
    }

    public override T GetBody<T>() => RigidBody as T;

    public override void AddVelocities(float delta)
    {
        if (RigidBody == null) return;

        PreviousVelocity = RigidBody.LinearVelocity;
        
        // Combine forces to reduce API calls
        Vector3 totalForce = (Acceleration * Speed) + Force + (RigidBody.Basis * LocalForce);
        
        if (!UseGravity)
        {
            totalForce += Gravity;
        }

        RigidBody.ApplyForce(totalForce);
    }

    protected override void MoveBody(float delta) 
    {
        // RigidBody movement is handled by the physics engine
    }

    private void HandleBodyState(PhysicsDirectBodyState3D state)
    {
        int contactCount = state.GetContactCount();
        for (int i = 0; i < contactCount; i++)
        {
            ulong colliderId = state.GetContactColliderId(i);
            var collider = InstanceFromId(colliderId) as CollisionObject3D;

            if (collider == null) continue;

            // Extract data while state is valid
            Vector3 worldPos = state.GetContactColliderPosition(i);
            Vector3 normal = state.GetContactLocalNormal(i);

            var relativeVelocity = RigidBody.LinearVelocity - state.GetContactColliderVelocityAtPosition(i);
            
            // Pass to the safe handler
            CallDeferred(MethodName.HandleCollisionInternal, collider, worldPos, normal, relativeVelocity);
        }
    }

    // Helper to keep HandleBodyState clean and type-safe
    private void HandleCollisionInternal(CollisionObject3D collider, Vector3 position, Vector3 normal, Vector3 relativeVelocity)
    {
        HandleCollision(collider, position, normal, relativeVelocity);
    }
}