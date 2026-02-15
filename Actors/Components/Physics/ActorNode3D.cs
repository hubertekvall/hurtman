using System;
using System.Collections.Generic;
using Godot;

namespace Hurtman.Actors.Components.Physics;

[GlobalClass, Tool]
public partial class ActorNode3D : Node3D, IActorComponent, IMessageHandler 
{
	public Actor Actor { get; set; }

	public void Setup()
	{
		
	}

	public void OnMessage(ActorMessage message)
	{
		
		if (message is TeleportMessage3D teleportMessage)
		{
			Position = teleportMessage.Position;
		}
	}

	
}
