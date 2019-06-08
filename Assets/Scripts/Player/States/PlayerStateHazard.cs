using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHazard : PlayerState
{

    private float hazardTimer;

    public PlayerStateHazard(Player p) : base(p) {
        hazardTimer = 0f;
    }

    public override void OnEnter()
    {
        Player.Stage.HazardColor();
        Player.Stage.UpdateActive = false;
    }

    public override void OnUpdate()
    {
        hazardTimer += Time.deltaTime;
        if(hazardTimer >= Player.Stage.hazardHitTime)
        {
            Player.RefreshDizzyTimer();
            ChangeState("Dizzy");
        }
    }

    public override void OnExit()
    {
        Player.Stage.IdleColor();
        Player.Stage.UpdateActive = true;
        hazardTimer = 0f;
    }
}
