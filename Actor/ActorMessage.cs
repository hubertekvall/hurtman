using Godot;
namespace Hurtman.Actor;

public partial class ActorMessage : RefCounted
{
    public Actor Owner { get; set; }
}

