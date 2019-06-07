using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerState
{
    public PlayerStateIdle(Player p) : base(p) { }

    public override void OnUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Player.transform.Translate(Vector2.right * horizontal * Player.speed * Time.deltaTime);

        float playerX = Player.transform.position.x;
        if(playerX >= Player.Stage.rightSide.position.x || playerX <= Player.Stage.leftSide.position.x)
        {
            ChangeState("WrapAround");
        }
    }
}
