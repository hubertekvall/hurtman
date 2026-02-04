using Godot;
using Godot.Collections;

namespace Hurtman.Actor;

[GlobalClass]
[Tool] 
public partial class CharacterActor : Actor
{
	private CharacterBody3D _characterBody;
	
	public override void _Ready()
	{
		base._Ready();
		
		// Auto-create CharacterBody3D if it doesn't exist
		if (Engine.IsEditorHint())
		{
			EnsureCharacterBodyExists();
		}
		else
		{
			// In-game, just cache the reference
			_characterBody = GetNodeOrNull<CharacterBody3D>("CharacterBody3D");
		}
	}

	private void EnsureCharacterBodyExists()
	{
		// Check if we already have a CharacterBody3D child
		_characterBody = GetNodeOrNull<CharacterBody3D>("CharacterBody3D");
		
		if (_characterBody == null)
		{
			// Create one automatically
			_characterBody = new CharacterBody3D
			{
				Name = "CharacterBody3D"
			};
			AddChild(_characterBody);
			_characterBody.Owner = GetTree().EditedSceneRoot; // Important for saving in editor!
			
			GD.Print($"Auto-created CharacterBody3D for {Name}");
		}
	}
}
