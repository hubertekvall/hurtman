using Godot;
using Hurtman.Actor.Components;

namespace Hurtman.Actor;



public partial class CollisionMessage : ActorMessage
{
    

    public CollisionObject3D Other { get; set; }
 
    public Vector3 CollisionPosition { get; set; }
    
    public Vector3 OtherNormal { get; set; }
}