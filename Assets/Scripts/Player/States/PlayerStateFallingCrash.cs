using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFallingCrash : PlayerState
{
    private float targetY;

    public PlayerStateFallingCrash(Player p) : base(p) { }

    public override void OnEnter()
    {
        targetY = Player.transform.position.y - Player.crashHeight;
    }

    public override void OnUpdate()
    {
        Player.transform.Translate(Vector3.up * -Player.jumpSpeed * Time.deltaTime);
        if(Player.transform.position.y <= targetY)
        {
            Player.transform.position = new Vector2(Player.transform.position.x, targetY);
            Player.RefreshDizzyTimer();
            ChangeState("Dizzy");
        }
    }
}
