using Godot;

namespace Hurtman.Actor;

[GlobalClass]
[Tool]
public partial class CharacterMovementComponent : MovementComponent
{
    private CharacterBody3D _body3D;

    [Export]
    public CharacterBody3D CharacterBody
    {
        get => _body3D;
        set => _body3D = value;
    }

    protected override CollisionObject3D Body3D => _body3D;

    public override void SetupBody()
    {
        if (_body3D is null)
        {
            _body3D = new CharacterBody3D();
            AddChild(_body3D);
            _body3D.SetOwner(GetTree().EditedSceneRoot);
        }
        
        _body3D.Position = Actor.Position;
        _body3D.Basis = Actor.Basis;
    }

    public override void InitializeVelocities()
    {
        var relativeVelocity = _body3D.Basis * LocalImpulse;
        var initialVelocity = Impulse + relativeVelocity;
        _body3D.Velocity = initialVelocity;
    }

    public override T GetBody<T>()
    {
        return _body3D as T;
    }

    protected override void MoveBody(float delta)
    {
        if (!_body3D.MoveAndSlide()) return;

        for (int c = 0; c < _body3D.GetSlideCollisionCount(); c++)
        {
            var collision = _body3D.GetSlideCollision(c);
            
            if (collision.GetCollider() is CollisionObject3D collider)
            {
                HandleCollision(
                    collider,
                    collision.GetPosition(),
                    collision.GetNormal(),
                    collision.GetColliderVelocity()
                );
            }
        }
    }
}