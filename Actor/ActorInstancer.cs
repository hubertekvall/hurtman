using System;
using Godot;
using Godot.Collections;
namespace Hurtman.Actor;

public partial class ActorInstancer3D : Node3D
{
    [Export]
    public PackedScene ActorScene { get; set; }
    
    [Export]
    public bool Local  { get; set; } = false;

    
    
    
    [Export]
    public Array<ActorComponent> ActorComponents { get; set; }
    
    private Actor Actor { get; set; }
    private bool Instanced { get; set; } = false;
    
    
    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is ActorComponent component)
            {
                component.Actor = Actor;
                ActorComponents.Add(component);
            }
        }
    }

    public Actor Instantiate()
    {
        var instance = ActorScene.Instantiate();
        if (instance is Actor actor)
        {
            if (Local)
            {
                AddChild(actor);
            }
            else
            {
                actor.Position = GlobalPosition;
                actor.Transform = GlobalTransform;
                GetViewport().AddChild(actor);
            }

   
            var duplicateInstance = Duplicate() as ActorInstancer3D;
            duplicateInstance!.Actor = actor;
            duplicateInstance.Instanced = true;
            
            actor.AddChild(duplicateInstance);
            
            return actor;
        }
        throw new Exception("Scene root must be of the Actor class");
    }


    public override void _Process(double delta)
    {
        if (Instanced == false) return;
        
        
        foreach (var component in ActorComponents)
        {
            component.ProcessTick(delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Instanced == false) return;

        foreach (var component in ActorComponents)
        {
            component.PhysicsTick(delta);
        }
        
    }
}