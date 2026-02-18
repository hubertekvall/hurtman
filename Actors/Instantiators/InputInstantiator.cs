using Godot;

namespace Hurtman.Actors.Instantiators;

public partial class InputInstantiator : Instantiator, IInputHandler
{
    public void OnInput(InputEvent inputEvent)
    {
        if (Input.IsKeyPressed(Key.Space))
        {
        }
    }
}