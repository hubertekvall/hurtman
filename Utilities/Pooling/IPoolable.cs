namespace Hurtman.Utilities.Pooling;

public interface IPoolable<T> where T : IPoolable<T>
{
    public ObjectPool<T> Pool { get; set; }
    public void QueueFree();
}