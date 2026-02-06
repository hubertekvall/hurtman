#nullable enable

using Godot;

namespace Hurtman.Actor;

public partial class CollisionMessage( CollisionObject3D? collider, Vector3 collisionPosition, Vector3 normal, Vector3 relativeVelocity = default) : ActorMessage
{
    public CollisionObject3D? Collider { get; set; } = collider;
    public Vector3 CollisionPosition { get; set; } = collisionPosition;
    public Vector3 Normal { get; set; } = normal;
    public Vector3 RelativeVelocity { get; set; } = relativeVelocity;
}