using Godot;
using Hurtman.Actors.Components.Physics.Casting;

namespace Hurtman.Actors.Components.Physics;

public partial class BufferedJump : Node, IActorComponent, IInputHandler, IPhysicsHandler
{
    [Export] public float TotalJumpForce { get; set; } = 10f;
    [Export] public float ImpulseFraction { get; set; } = 0.6f; // 0-1, portion spent on initial impulse
    [Export] public float MaxHoldTime { get; set; } = 0.3f;
    [Export] public StringName JumpAction { get; set; } = "jump";

    public IPhysicsComponent3D PhysicsComponent { get; set; }
    public ActorRaycast ActorRaycast { get; set; }
    public Actor Actor { get; set; }

    private bool _isHolding = false;
    private float _holdTimer = 0f;
    private float _remainingForce = 0f;
    private bool _jumpedThisFrame = false;

    public void Setup()
    {
        PhysicsComponent = Actor.GetComponent<IPhysicsComponent3D>();
        ActorRaycast = Actor.GetComponent<ActorRaycast>();
    }

    public void OnInput(InputEvent @event)
    {
        if (@event.IsActionPressed(JumpAction) && ActorRaycast.HitDistance < 0.1f)
        {
            _isHolding = true;
            _holdTimer = 0f;
            _remainingForce = TotalJumpForce;
            
            JumpImpulse();
        }

        if (@event.IsActionReleased(JumpAction))
        {
            _isHolding = false;
            _remainingForce = 0f;
        }
    }


    private void JumpImpulse()
    {
        float impulse = _remainingForce * ImpulseFraction;
        _remainingForce -= impulse;
        PhysicsComponent.ApplyImpulse(Vector3.Up * impulse);
    }


    public void PhysicsTick(float delta)
    {
        if (_isHolding && _remainingForce > 0f)
        {
            float holdForce = _remainingForce / MaxHoldTime;
            float frameForce = holdForce * delta;

            frameForce = Mathf.Min(frameForce, _remainingForce);
            _remainingForce -= frameForce;

            PhysicsComponent.ApplyForce(Vector3.Up * frameForce / delta);
            _holdTimer += delta;

            if (_remainingForce <= 0f)
                _isHolding = false;
        }
    }
}