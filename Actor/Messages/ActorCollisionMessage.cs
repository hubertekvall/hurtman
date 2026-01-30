using Godot;

namespace Hurtman.Actor;


public partial class ActorCollisionMessage(
    Actor? actor,
    CollisionObject3D? collider,
    Vector3 collisionPosition,
    Vector3 normal,
    Vector3 relativeVelocity = default)
    : CollisionMessage(collider, collisionPosition, normal, relativeVelocity)
{
    
    public Actor CollidingActor => actor;
}