using System;
using Godot;
using Hurtman.Actor;
namespace Hurtman.StateMachine;

[GlobalClass]
public partial class StateMachine : Node, IActorComponent
{
	private State? CurrentState { get; set; }
	private State? PreviousState { get; set; }

	
	[Signal] 
	public delegate void StateExitEventHandler(State state);
	
	[Signal] 
	public delegate void StateEnterEventHandler(State state);

	public IActor Actor { get; set; }

	public  void PhysicsTick(float delta)
	{
		CurrentState?.Run(delta);
	}

	public void ProcessTick(float delta) { }

	public void OnMessage(ActorMessage message) { }

	public void Setup() { }


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
