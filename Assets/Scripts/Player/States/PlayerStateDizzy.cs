using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDizzy : PlayerState
{
    public PlayerStateDizzy(Player p) : base(p) { }

    public override void OnEnter()
    {
        if(Player.Lane == -1)
        {
            Player.Stage.OnLoseLife();
            if(GameManager.Instance.Lives == 0)
            {
                Player.Stage.OnGameOver();
                ChangeState("GameOver");
            }
        }
    }

    public override void OnUpdate()
    {
        Player.DizzyTimer += Time.deltaTime;
        if(Player.DizzyTimer >= Player.DizzyTime)
        {
            ChangeState("Idle");
        }
        if (Player.CheckFloorHole())
        {
            ChangeState("Falling");
        }
    }
}
