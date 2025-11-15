#nullable enable

using Godot;

namespace Hurtman.Actor;



public partial class CollisionMessage(CollisionObject3D collider, Vector3 collisionPosition, Vector3 normal)  : ActorMessage
{
    


    
    public CollisionObject3D? Collider { get; set; } = collider;
    
    public Vector3 CollisionPosition { get; private set; } = collisionPosition;

    public Vector3 Normal { get; set; } = normal;
}