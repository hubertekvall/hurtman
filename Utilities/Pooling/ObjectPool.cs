using System;
using System.Collections.Generic;
using Godot;
using Hurtman.Actors;

namespace Hurtman.Utilities.Pooling;



public class ObjectPool<T> where T : IPoolable<T>
{
	private readonly Queue<T> _availableObjects = new();
	private readonly HashSet<T> _activeObjects = new();
	
	private static readonly Dictionary<String, ObjectPool<T>> Pools = new ();
	
	public static ObjectPool<T> GetPool(String name)
	{
		if (Pools.TryGetValue(name, out ObjectPool<T> value)) return value;
		
		value = new ObjectPool<T>();
		GD.Print(value);
		Pools[name] = value;
		return value;
	}
	
	
	public bool HasAvailableObjects() => _availableObjects.Count > 0;
	public bool HasActiveObjects() => _activeObjects.Count > 0;
	public int AvailableObjectCount => _availableObjects.Count;
	public int ActiveObjectCount => _activeObjects.Count;
	
	
	public T Get(Func<T> creationAction)
	{
	
		
		T obj;
		
		if(_availableObjects.Count == 0) obj = creationAction.Invoke();
		else obj = _availableObjects.Dequeue();
		
	
		
		_activeObjects.Add(obj);
		return obj;
	}

	public bool Return(T obj)
	{

		
		if (!_activeObjects.Remove(obj)) return false;
		_availableObjects.Enqueue(obj);
		return true;
	}
}
