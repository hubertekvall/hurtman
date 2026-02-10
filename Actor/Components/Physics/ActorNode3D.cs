using System;
using System.Collections.Generic;
using Godot;

namespace Hurtman.Actor.Components.Physics;

[GlobalClass, Tool]
public partial class ActorNode3D : Node3D, IActorComponent, IMessageHandler 
{
	public IActor Actor { get; set; }
	public void Setup()
	{
	}

	public void OnMessage(ActorMessage message)
	{
		
		if (message is TeleportMessage3D teleportMessage)
		{
			
			GD.Print(teleportMessage.Position);
			GlobalPosition = teleportMessage.Position;
		}
	}

	
}
