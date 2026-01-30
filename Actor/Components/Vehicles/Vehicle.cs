using Godot;

namespace Hurtman.Actor.Vehicles;

public partial class Vehicle : RigidBodyMovementComponent
{
    [Export] public float EngineForce { get; set; }
    [Export] public float Steering { get; set; }
}