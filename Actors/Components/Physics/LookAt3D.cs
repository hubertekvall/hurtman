using Godot;
namespace Hurtman.Actors.Components.Physics;

public partial class LookAt3D : Node3D, IActorComponent, IProcessHandler
{
    public Vector3 LookVector { get; set; }
    
    public void ProcessTick(float delta)
    {
       LookTransform();
    }
    protected virtual void LookTransform()
    {
        Vector3 direction = LookVector.Normalized();
        Vector3 up = Mathf.Abs(direction.Dot(Vector3.Up)) > 0.99f ? Vector3.Forward : Vector3.Up;
        LookAt(GlobalPosition + direction, up);
    }
    public Actor Actor { get; set; }
    public void Setup()
    {
    }
}