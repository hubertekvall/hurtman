using System;
using Godot;
using Godot.Collections;
namespace Hurtman.StateMachine;

[GlobalClass]
public partial class State(Dictionary<String, Variant> blackboard) : Node
{
    private Dictionary<String, Variant> Blackboard { get; set; } = blackboard;



    public virtual void Enter() { }
    public virtual void Run(float delta) { }
    public virtual void Exit() { }

    /// <summary>
    /// Whether this state allows the state machine to transition away from it to a specific target state.
    /// Returning false blocks the transition unless it's forced.
    /// </summary>
    public virtual bool CanTransition(State targetState) => true;

    /// <summary>
    /// Requests the state machine to transition to a new state.
    /// Respects CanTransition() checks.
    /// </summary>
    private void RequestTransition(State nextState)
    {
        StateMachine?.Transition(nextState);
    }

    /// <summary>
    /// Requests a forced transition, ignoring CanTransition().
    /// </summary>
    private void RequestForceTransition(State nextState)
    {
        StateMachine?.Transition(nextState, force: true);
    }

    internal StateMachine? StateMachine { get; set; }
}
