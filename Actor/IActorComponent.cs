using Godot;

namespace Hurtman.Actor;

public interface IActorComponent
{
    public IActor Actor {get; set;}
    public void Setup();
}