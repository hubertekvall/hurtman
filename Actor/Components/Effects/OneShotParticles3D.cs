using System.Threading.Tasks;
using Godot;

namespace Hurtman.Actor.Components.Effects;

public partial class OneShotParticles3D : CpuParticles3D, IActorComponent
{
	
	public IActor Actor { get; set; }
	
	public  void  Setup()
	{
		OneShot = true;
		Emitting = true;
	}
}
