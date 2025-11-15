using Godot;

namespace Hurtman.Actor;

public partial class ActorCollisionMessage(Actor other, CollisionObject3D collider, Vector3 collisionPosition, Vector3 normal) : CollisionMessage(collider, collisionPosition, normal)
{
    public Actor? Other { get; set; } = other;
    
    
    
}