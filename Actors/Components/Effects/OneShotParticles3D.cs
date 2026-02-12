using System.Threading.Tasks;
using Godot;

namespace Hurtman.Actors.Components.Effects;

public partial class OneShotParticles3D : CpuParticles3D, IActorComponent
{
	
	public Actor Actor { get; set; }
	
	public  void  Setup()
	{
		OneShot = true;
		Emitting = true;
	}
}
