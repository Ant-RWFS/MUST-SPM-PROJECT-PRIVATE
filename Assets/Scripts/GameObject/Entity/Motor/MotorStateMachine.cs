using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorStateMachine
{
    public MotorState currentState { get; private set; }

    public void Initialize(MotorState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(MotorState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
