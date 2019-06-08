using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFalling : PlayerState
{
    private float targetY;

    public PlayerStateFalling(Player p) : base(p) { }

    public override void OnEnter()
    {
        targetY = Player.transform.position.y - Player.jumpHeight;
        Player.Stage.SlowHoles();
    }

    public override void OnUpdate()
    {
        Player.transform.Translate(Vector2.up * -Player.jumpSpeed * Time.deltaTime);
        if(Player.transform.position.y <= targetY)
        {
            Player.transform.position = new Vector2(Player.transform.position.x, targetY);
            Player.RefreshDizzyTimer();
            ChangeState("Dizzy");
        }
    }

    public override void OnExit()
    {
        Player.Stage.NormalSpeedHoles();
    }
}
