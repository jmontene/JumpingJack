using System;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachineState
{
    IStateMachine Owner
    {
        get;
        set;
    }

    void OnEnter();
    void OnExit();
    void OnUpdate();
    void ChangeState(string key);
}
