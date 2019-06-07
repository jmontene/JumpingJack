using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCrashing : PlayerState
{
    private float targetY;
    private bool crashed = false;
    private float crashTimer;
    private float flashTime = 0.1f;
    private int flashStage = 1;

    public PlayerStateCrashing(Player p) : base(p) {
        crashTimer = 0f;
    }

    public override void OnEnter()
    {
        targetY = Player.transform.position.y + Player.crashHeight;
        Player.Stage.UpdateActive = false;
    }

    public override void OnUpdate()
    {
        if (!crashed)
        {
            Player.transform.Translate(Vector2.up * Player.jumpSpeed * Time.deltaTime);
            if (Player.transform.position.y >= targetY)
            {
                Player.Stage.HurtColor();
                crashed = true;
            }
        }
        else
        {
            crashTimer += Time.deltaTime;
            switch (flashStage)
            {
                case 1:
                    if (crashTimer >= Player.crashTime)
                    {
                        Player.Stage.IdleColor();
                        flashStage = 2;
                        crashTimer = 0f;
                    }
                    break;
                case 2:
                    if (crashTimer >= flashTime)
                    {
                        Player.Stage.HurtColor();
                        flashStage = 3;
                        crashTimer = 0f;
                    }
                    break;
                case 3:
                    if (crashTimer >= Player.crashTime)
                    {
                        Player.Stage.IdleColor();
                        ChangeState("FallingCrash");
                    }
                    break;
            }
        }
    }

    public override void OnExit()
    {
        crashed = false;
        crashTimer = 0f;
        flashStage = 1;
        Player.Stage.UpdateActive = true;
    }
}
