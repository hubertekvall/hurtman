namespace Hurtman.Actor;

public interface ISubComponent
{
    IActorComponent ParentComponent { get; set; }
    public void Setup();
}