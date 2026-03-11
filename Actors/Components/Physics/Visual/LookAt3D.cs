using Godot;

namespace Hurtman.Actors.Components.Physics.Visual;

public partial class LookAt3D : Node3D, IActorComponent, IProcessHandler
{
    [Export]
    public float EasingValue { get; set; }
    
    protected Vector3 LookVector { get; set; }
    
    public void ProcessTick(float delta)
    {
       LookTransform(delta);
    }
    protected virtual void LookTransform(float delta)
    {
        var direction = LookVector.Normalized();
        var up = Mathf.Abs(direction.Dot(Vector3.Up)) > 0.99f ? Vector3.Forward : Vector3.Up;

        var xform = GlobalTransform.LookingAt(GlobalPosition + direction, up);

        GlobalTransform = GlobalTransform.InterpolateWith(xform, EasingValue * delta);
    }
    public Actor Actor { get; set; }
 
}