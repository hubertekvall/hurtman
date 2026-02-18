using Godot;

namespace Hurtman.Actors.Components.Physics.AttractorsRepulsors;

[GlobalClass, Tool]
public partial class VortexAffector : Affector
{
	/// <summary>
	/// How strongly objects are pulled inward toward the vortex center.
	/// Negative values push outward (centrifugal).
	/// </summary>
	[Export] public float RadialDamping { get; set; } = 0.3f;

	/// <summary>
	/// The world-space axis the vortex spins around. Defaults to the node's up axis.
	/// </summary>
	[Export] public Vector3 SpinAxis { get; set; } = Vector3.Up;

	/// <summary>
	/// If true, uses the node's local up axis instead of the exported SpinAxis.
	/// </summary>
	[Export] public bool UseLocalUp { get; set; } = true;
	
	



	protected override Vector3 CalculateForceVector(Vector3 direction, float distance, Vector3 velocity, float falloff)
	{
		var axis = UseLocalUp ? GlobalTransform.Basis.Y.Normalized() : SpinAxis.Normalized();

		// Tangent is perpendicular to both the inward direction and the spin axis —
		// this is what makes objects orbit rather than just fly in or out
		var tangent = axis.Cross(direction).Normalized();

		// Inward component pulls the object toward the center (or pushes if negative)
		var radialVelocity = velocity.Dot(direction) * direction;


		// Combine tangential spin and inward pull, both scaled by the radial falloff
		return (tangent * Force) * falloff + (-radialVelocity * (Force * RadialDamping));
	}
}
