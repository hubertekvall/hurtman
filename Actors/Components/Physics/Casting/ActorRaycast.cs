using Godot;

namespace Hurtman.Actors.Components.Physics.Casting;

[Tool, GlobalClass]
public partial class ActorRaycast : RayCast3D, IActorComponent, IPhysicsHandler
{
    public Actor Actor { get; set; }
    public float HitDistance { get; private set; }
    public void Setup()
    {
        Enabled = false;
    }


    public void PhysicsTick(float delta)
    {
        ForceRaycastUpdate();

        if (!IsColliding()) return;
        
        HitDistance = GlobalPosition.DistanceTo(GetCollisionPoint());   
        GD.Print(HitDistance);
    }
}