using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Hurtman.Actors;

namespace Hurtman.Utilities.Pooling;



public class ObjectPool<T> where T : IPoolable<T>
{
	private readonly Queue<T> _availableObjects = new();
	private readonly HashSet<T> _activeObjects = new();
	
	private static readonly Dictionary<string, ObjectPool<T>> Pools = new ();
	
	public static ObjectPool<T> GetPool(string name)
	{
		var pool = Pools.GetValueOrDefault(name) ?? GeneratePool(name);
		pool._poolUsers += 1;
		return pool;
	}


	private static ObjectPool<T> GeneratePool(string name)
	{
		var value = new ObjectPool<T>();
		Pools[name] = value;
		return value;
	}
	
	public bool HasAvailableObjects() => _availableObjects.Count > 0;
	public bool HasActiveObjects() => _activeObjects.Count > 0;
	public int AvailableObjectCount => _availableObjects.Count;
	public int ActiveObjectCount => _activeObjects.Count;

	private int _poolUsers;
	
	public T Get(Func<T> creationAction)
	{
		T obj;

		if (_availableObjects.Count == 0)
		{
			GD.Print("CREATED A NEW COPY", AvailableObjectCount);
			obj = creationAction.Invoke();
		}
		else
		{
			
			GD.Print("USED A POOLED COPY");
			obj = _availableObjects.Dequeue();
		}
		
		
		
		_activeObjects.Add(obj);
		return obj;
	}


	public void Warmup(int amount, Func<T> creationAction)
	{
		for (int i = 0; i < amount; i++)
		{
			_availableObjects.Enqueue(creationAction.Invoke());
		}
		
	}
	
	
	public bool Return(T obj)
	{
		if (!_activeObjects.Remove(obj)) return false;
		_availableObjects.Enqueue(obj);


		
		return true;
	}


	public void Clean()
	{
		_poolUsers -= 1;
		if (_poolUsers > 0) return;
		
		foreach (var obj in _availableObjects.Concat(_activeObjects))
		{
			obj.QueueFree();
		}
		
		_availableObjects.Clear();
		_activeObjects.Clear();
	}
}
