using Godot;

namespace Hurtman.Actor;

public abstract partial class CollisionComponent : ActorComponent
{
    
    protected CollisionObject3D Body { get; set; }
}