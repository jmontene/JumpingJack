﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : IStateMachine
{
    private Dictionary<string, IStateMachineState> states;
    private IStateMachineState currentState;

    public StateMachine()
    {
        states = new Dictionary<string, IStateMachineState>();
        currentState = null;
    }

    public void AddState(string key, IStateMachineState state)
    {
        state.Owner = this;
        states[key] = state;
    }

    public void ChangeState(string key)
    {
        if (!states.ContainsKey(key))
        {
            Debug.LogError("Invalid state machine key");
            return;
        }

        if (currentState != null) currentState.OnExit();
        currentState = states[key];
        currentState.OnEnter();
    }

    public void RemoveState(string key)
    {
        states.Remove(key);
    }

    public void Start(string key)
    {
        ChangeState(key);
    }

    public void UpdateMachine()
    {
        currentState.OnUpdate();
    }
}
