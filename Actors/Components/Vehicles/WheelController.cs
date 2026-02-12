using Godot;
using Hurtman.Actors.Vehicles;


namespace Hurtman.Actors.Vehicles;

public partial class WheelController : RayCast3D
{
    
    [Export]
    public Vehicle Vehicle;
    
    [Export] public RigidBody3D ParentBody { get; set; }
    [Export] public float WheelRadius { get; set; } = 0.3f;
    [Export] public MeshInstance3D Mesh { get; set; }
    
    // Suspension parameters
    [ExportGroup("Suspension")]
    [Export] public float SuspensionStrength { get; set; } = 50.0f;
    [Export] public float DampingRatio { get; set; } = 5.0f;
    [Export] public float RestDistance { get; set; } = 0.3f;
    
    // Wheel force parameters
    [ExportGroup("Wheel Forces")]
    [Export(PropertyHint.Range, "0.0, 1.0")] public float SteeringFactor { get; set; } = 1.0f;
    [Export(PropertyHint.Range, "0.0, 1.0")] public float RollFactor { get; set; } = 1.0f;  
    [Export] public float GripFactor { get; set; } = 1.0f;
    [Export] public Curve VelocityTractionCurve { get; set; }
    
    // Internal variables
    private bool isGrounded = false;
    
    public override void _Ready()
    {
        TargetPosition = new Vector3(0, -(RestDistance + WheelRadius), 0);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        ForceRaycastUpdate();
        
        AnimateMesh((float)delta);
        
        if (!IsColliding()) return;
        
        HandleSuspension((float)delta);
        HandleSteering((float)delta);
        HandleAcceleration((float)delta);
    }
    
    private void AnimateMesh(float delta)
    {
        float offset = RestDistance;
        
        if (IsColliding())
        {
            float hitDistance = GetCollisionPoint().DistanceTo(GlobalPosition);
            offset = hitDistance - WheelRadius;
        }
        
        if (Mesh != null)
        {
            Vector3 meshPos = Mesh.Position;
            meshPos.Y = Mathf.Lerp(meshPos.Y, -offset, 10.0f * delta);
            Mesh.Position = meshPos;
        }
    }
    
    private void HandleSuspension(float delta)
    {
        Vector3 springDirection = GlobalBasis.Y;
        Vector3 tireWorldVelocity = GetVelocityAtPoint(ParentBody, GlobalPosition);
        float hitDistance = GetCollisionPoint().DistanceTo(GlobalPosition);
        float offset = (RestDistance) - hitDistance + WheelRadius;
        
        float velocity = springDirection.Dot(tireWorldVelocity);
        float force = (offset * SuspensionStrength ) - (velocity  * DampingRatio);
        
        ApplyForceToParent(force * springDirection);
    }
    
    private void HandleSteering(float delta)
    {
        // Build target basis aligned with parent
        Basis newBasis = new Basis();
        newBasis.Y = Vector3.Up;  // Keep the up vector (suspension direction)
        newBasis.Z = -Vector3.Forward.Rotated(newBasis.Y,  Vehicle.Steering * SteeringFactor);  // Align forward with parent
        newBasis.X = newBasis.Y.Cross(newBasis.Z).Normalized();  // Right vector
        
        // Smoothly interpolate to target basis
        Basis = Basis.Slerp(newBasis.Orthonormalized(), 2.0f * delta);
        
        Vector3 steeringDirection = GlobalBasis.X;
        Vector3 tireWorldVelocity = GetVelocityAtPoint(ParentBody, GlobalPosition);
        float steeringVelocity = steeringDirection.Dot(tireWorldVelocity);
        float normalizedTireVelocity = tireWorldVelocity.LimitLength(50.0f).Length() / 50.0f;
        float traction = VelocityTractionCurve.Sample(normalizedTireVelocity);
        float desiredVelocityChange = -steeringVelocity * GripFactor * traction;
        float desiredAcceleration = desiredVelocityChange / delta;
        
        ApplyForceToParent(steeringDirection * desiredAcceleration);
    }
    
    private void HandleAcceleration(float delta)
    {
        ParentBody.ApplyForce(
            -GlobalBasis.Z * Vehicle.EngineForce * RollFactor, 
            ParentBody.GlobalPosition - GlobalPosition
        );
    }
    
    /// <summary>
    /// Calculate velocity at a specific world point on the rigid body
    /// </summary>
    private Vector3 GetVelocityAtPoint(RigidBody3D body, Vector3 worldPoint)
    {
        Vector3 offset = worldPoint - body.GlobalPosition;
        Vector3 velocity = body.LinearVelocity + body.AngularVelocity.Cross(offset);
        return velocity;
    }
    
    private void ApplyForceToParent(Vector3 force)
    {
        ParentBody.ApplyForce(force, GlobalPosition - ParentBody.GlobalPosition);
    }
}