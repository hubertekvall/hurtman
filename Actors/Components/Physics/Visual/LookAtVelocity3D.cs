namespace Hurtman.Actors.Components.Physics.Visual;

public partial class LookAtVelocity3D : Visual.LookAt3D, ISetupHandler
{
	
	private IPhysicsComponent3D PhysicsComponent { get; set; }
	public void Setup()
	{
		PhysicsComponent = Actor.GetComponent<IPhysicsComponent3D>();
	}

	
	protected override void LookTransform(float delta)
	{
		if (PhysicsComponent == null) return;
		
		var velocity = PhysicsComponent.Velocity;
		if (velocity.LengthSquared() < 0.01f) return;
		LookVector = velocity;
		
		base.LookTransform(delta);

	}
}
