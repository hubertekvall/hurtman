using Godot;

namespace Hurtman.Actor;



public partial class CollisionMessage(Actor other, Vector3 collisionPosition, Vector3 normal)  : ActorMessage
{
    

    public Actor Other { get; set; } = other;

    public Vector3 CollisionPosition { get; private set; } = collisionPosition;

    public Vector3 Normal { get; set; } = normal;
}