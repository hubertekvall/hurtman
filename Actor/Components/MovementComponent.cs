using Godot;

namespace Hurtman.Actor;

public abstract partial class MovementComponent : ActorComponent
{
    [Export] public Vector3 Gravity { get; set; }
    [Export] public Vector3 LocalImpulse { get; set; }
    [Export] public Vector3 LocalForce { get; set; }
    [Export] public Vector3 Impulse { get; set; }
    [Export] public Vector3 Force { get; set; }
    [Export] public float Drag { get; set; } = 0.95f;

    public float Speed { get; set; }
    public Vector3 Acceleration { get; set; }
    public Vector3 PreviousVelocity { get; set; }
    public Vector3 LookVector { get; set; }

    // Abstract property that subclasses must provide
    protected abstract CollisionObject3D Body3D { get; }

    protected override void Setup()
    {
        SetupBody();
        InitializeVelocities();
    }

    public abstract void SetupBody();
    public abstract void InitializeVelocities();
    public abstract T GetBody<T>() where T : CollisionObject3D;
    
    // These are now implemented in the base class using the abstract Body3D property
    public virtual Vector3 GetBodyVelocity()
    {
        return Body3D switch
        {
            CharacterBody3D character => character.Velocity,
            RigidBody3D rigid => rigid.LinearVelocity,
            _ => Vector3.Zero
        };
    }

    public virtual void SetBodyVelocity(Vector3 velocity)
    {
        switch (Body3D)
        {
            case CharacterBody3D character:
                character.Velocity = velocity;
                break;
            case RigidBody3D rigid:
                rigid.LinearVelocity = velocity;
                break;
        }
    }

    public virtual Vector3 GetBodyPosition() => Body3D.Position;
    public virtual Basis GetBodyBasis() => Body3D.Basis;
    public virtual void SetBodyTransform(Transform3D transform) => Body3D.GlobalTransform = transform;
    public virtual Transform3D GetBodyGlobalTransform() => Body3D.GlobalTransform;

    public override void PhysicsTick(float delta)
    {
        AddVelocities(delta);
        LookAt(delta);
        MoveBody(delta);
    }

    public override void ProcessTick(float delta)
    {
        Actor.Position = GetBodyPosition();
        Actor.Basis = GetBodyBasis();
    }

    public override void OnMessage(ActorMessage message) { }

    public virtual void AddVelocities(float delta)
    {
        PreviousVelocity = GetBodyVelocity();
        var currentVelocity = GetBodyVelocity();
        var bodyBasis = GetBodyBasis();
        
        var relativeConstantVelocity = bodyBasis * LocalForce;
        currentVelocity += (Acceleration * Speed) * delta;
        currentVelocity += Gravity * delta;
        currentVelocity += relativeConstantVelocity * delta;
        currentVelocity += Force * delta;
        currentVelocity = currentVelocity.Lerp(Vector3.Zero, Drag * delta);
        
        SetBodyVelocity(currentVelocity);
    }

    protected abstract void MoveBody(float delta);

    /// <summary>
    /// Generalized collision handler that works for both kinematic and rigid body collisions
    /// </summary>
    protected virtual void HandleCollision(CollisionObject3D collider, Vector3 collisionPosition, Vector3 normal, Vector3 relativeVelocity)
    {
        if (collider == null) return;

        var parent = collider.GetParent();
        
        

        CollisionMessage collisionMessage;

        if (parent is Actor otherActor)
        {
            collisionMessage = new ActorCollisionMessage(
                otherActor,
                collider,
                collisionPosition,
                normal,
                relativeVelocity
            );

        }
        else
        {
            collisionMessage = new CollisionMessage(
                collider,
                collisionPosition,
                normal,
                relativeVelocity
            );

        }

        Actor.SendMessage(collisionMessage, Actor);
    }

    public virtual void LookAt(float delta)
    {
        if (LookVector.IsZeroApprox())
            return;

        var normalizedLook = LookVector.Normalized();
        var forwardVector = -normalizedLook;

        Vector3 refUp = (Mathf.Abs(forwardVector.Dot(Vector3.Up)) > 0.999f)
            ? Vector3.Forward
            : Vector3.Up;

        var rightVector = refUp.Cross(forwardVector);
        
        if (!rightVector.IsZeroApprox())
        {
            var upVector = forwardVector.Cross(rightVector);
            var newBasis = new Basis(rightVector.Normalized(), upVector.Normalized(), forwardVector.Normalized());
            var xForm = new Transform3D(newBasis.Orthonormalized(), GetBodyPosition());

            SetBodyTransform(GetBodyGlobalTransform().InterpolateWith(xForm, 10.0f * delta));
        }

        var velocity = GetBodyVelocity();
        if (!velocity.IsZeroApprox())
            LookVector = velocity.Normalized();
    }
}