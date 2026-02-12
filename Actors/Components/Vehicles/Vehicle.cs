using Godot;
using Hurtman.Actors.Components;
using ActorRigidBody = Hurtman.Actors.Components.Physics.ActorRigidBody;
using Physics_ActorRigidBody = Hurtman.Actors.Components.Physics.ActorRigidBody;

namespace Hurtman.Actors.Vehicles;

public partial class Vehicle : Physics_ActorRigidBody
{
    [Export] public float EngineForce { get; set; }
    [Export] public float Steering { get; set; }
}