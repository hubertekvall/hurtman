namespace Hurtman.Actors;

public interface ISubComponent
{
    IActorComponent ParentComponent { get; set; }
}