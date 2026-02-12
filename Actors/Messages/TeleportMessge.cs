using Godot;

namespace Hurtman.Actors;

public partial class TeleportMessage3D(Vector3 position) : ActorMessage
{
    public Vector3 Position { get; set; } = position;
}