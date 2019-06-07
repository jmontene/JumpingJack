using System;
using System.Collections.Generic;

public interface IStateMachine
{
    void Start(string key);
    void UpdateMachine();
    void ChangeState(string key);
    void AddState(string key, IStateMachineState state);
    void RemoveState(string key);
}
