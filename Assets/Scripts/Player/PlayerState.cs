using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : IStateMachineState
{
    protected IStateMachine _owner;
    public IStateMachine Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    protected Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public PlayerState(Player p)
    {
        Player = p;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }

    public void ChangeState(string key)
    {
        Owner.ChangeState(key);
    }
}

