using Godot;
using Hurtman.Actor.Components;
using ActorRigidBody = Hurtman.Actor.Components.Physics.ActorRigidBody;

namespace Hurtman.Actor.Vehicles;

public partial class Vehicle : ActorRigidBody
{
    [Export] public float EngineForce { get; set; }
    [Export] public float Steering { get; set; }
}