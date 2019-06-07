using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateWrapAround : PlayerState {

    private bool movingIn = false;
    private float targetX;
    private int direction;

    public PlayerStateWrapAround(Player p) : base(p) { }

    public override void OnEnter()
    {

        float playerX = Player.transform.position.x;
        float playerLeft = Player.leftSide.position.x;
        float playerRight = Player.rightSide.position.x;
        float right = Player.Stage.rightSide.position.x;
        float left = Player.Stage.leftSide.position.x;

        if(playerX >= right)
        {
            targetX = playerX + (playerX - playerLeft);
            direction = 1;
        }else if(playerX <= left)
        {
            targetX = -playerRight;
            direction = -1;
        }
    }

    public override void OnUpdate()
    {
        Player.transform.Translate(Vector2.right * direction * Player.speed * Time.deltaTime);
        if (direction == 1 && Player.transform.position.x >= targetX)
        {
            if (!movingIn)
            {
                Wrap();
                movingIn = true;
                targetX = Player.rightSide.position.x;
            }
            else
            {
                ChangeState("Idle");
            }
        }
        else if (direction == -1 && Player.transform.position.x <= targetX)
        {
            if (!movingIn)
            {
                Wrap();
                movingIn = true;
                targetX = Player.leftSide.position.x;
            }
            else
            {
                ChangeState("Idle");
            }
        }
    }

    public override void OnExit()
    {
        movingIn = false;
    }

    private void Wrap()
    {
        float playerX = Player.transform.position.x;
        float playerLeft = Player.leftSide.position.x;
        float playerRight = Player.rightSide.position.x;
        float right = Player.Stage.rightSide.position.x;
        float left = Player.Stage.leftSide.position.x;

        if (direction == 1)
        {
            Player.transform.position = new Vector2(left - (playerX - playerLeft), Player.transform.position.y);
        }
        else
        {
            Player.transform.position = new Vector2(right + (playerRight - playerX), Player.transform.position.y);
        }
    }
}
