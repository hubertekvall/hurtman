using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Hurtman.Actors;

namespace Hurtman.Utilities.Pooling;

public partial class CachePool : Node
{
    private readonly Queue<Actor> _cache = new();

    private bool _refilling = false;

    private readonly Func<Actor> _creationAction;

    private int _capacity;

    public CachePool(int capacity, Func<Actor> creationAction)
    {
        this._creationAction = creationAction;
        this._capacity = capacity;

        for (var i = 0; i < capacity; i++)
        {
            _cache.Enqueue(creationAction());
        }
    }


    public Actor Get()
    {
        if (_cache.Count == 0) _ = Refill();
        return _cache.TryDequeue(out var item) ? item : _creationAction();
    }

    private async Task Refill()
    {
        if (_refilling) return;
        _refilling = true;
        
        try
        {
            while (_cache.Count < _capacity)
            {
                _cache.Enqueue(_creationAction());
                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }
        }
        finally
        {
            _refilling = false;
        }
    }


    public override void _Notification(int what)
    {
        if (what == Node.NotificationPredelete)
        {
            foreach (var actor in _cache)
            {
                actor.QueueFree();
            }

            _cache.Clear();
        }
    }
}