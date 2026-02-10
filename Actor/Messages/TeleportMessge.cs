using Godot;

namespace Hurtman.Actor;

public partial class TeleportMessage3D(Vector3 position) : ActorMessage
{
    public Vector3 Position { get; set; } = position;
}