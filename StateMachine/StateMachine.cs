using System;
using Godot;
using Hurtman.Actor;
namespace Hurtman.StateMachine;

public partial class State(Godot.Collections.Dictionary<String, Variant> blackboard) : Node
{
	private Godot.Collections.Dictionary<String, Variant> Blackboard { get; set; } = blackboard;



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

public partial class StateMachine : ActorComponent
{
	private State? CurrentState { get; set; }
	private State? PreviousState { get; set; }

	
	[Signal] public delegate void StateExitEventHandler(State state);
	[Signal] public delegate void StateEnterEventHandler(State state);
	
	public override void PhysicsTick(float delta)
	{
		CurrentState?.Run(delta);
	}

 

	/// <summary>
	/// Transitions to a new state.
	/// If force = false, respects CanTransition() check.
	/// If force = true, immediately transitions regardless of CanTransition().
	/// </summary>
	public void Transition(State nextState, bool force = false)
	{
		if (nextState == null)
		{
			GD.PushError("Cannot transition to null state");
			return;
		}

		if (nextState == CurrentState)
		{
			return;
		}

		// Check if transition is allowed (unless forced)
		if (!force && CurrentState != null && !CurrentState.CanTransition(nextState))
		{
			return;
		}

		// Exit old state
		if (CurrentState != null)
		{
			CurrentState.Exit();
			EmitSignalStateExit(CurrentState);
			PreviousState = CurrentState;
		}

		// Enter new state
		CurrentState = nextState;
		CurrentState.StateMachine = this;
		EmitSignalStateEnter(CurrentState);
		CurrentState.Enter();
	}

	/// <summary>
	/// Returns the current active state, or null if none.
	/// </summary>
	public State? GetCurrentState() => CurrentState;

	/// <summary>
	/// Returns the previous state, or null if none.
	/// </summary>
	public State? GetPreviousState() => PreviousState;
}
