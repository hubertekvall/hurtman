using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace Hurtman.Actor;

public partial class Actor : Node3D
{
    [Signal]
    public delegate void OnCreationEventHandler();

    [Signal]
    public delegate void OnDeathEventHandler();

    [Signal]
    public delegate void OnCollisionEventHandler(CollisionMessage message);
    
    [Signal]
    public delegate void OnHitEventHandler(DamageMessage message);

    [Signal]
    public delegate void OnMessageEventHandler(ActorMessage message);


    public override void _Ready()
    {
        CallDeferred("_PostReady");
        EmitSignalOnCreation();
    }

    public virtual void _PostReady() {}
    

    public void ReceiveMessage(ActorMessage message)
    {
        if (message is DamageMessage damageMessage)
        {   
            EmitSignalOnHit(damageMessage);
        }
        else if (message is CollisionMessage collisionMessage)
        {
            EmitSignalOnCollision(collisionMessage);
        }
    }


    public void Kill()
    {
        if (IsQueuedForDeletion()) return;
        EmitSignalOnDeath();
        
        QueueFree();
    }
}








