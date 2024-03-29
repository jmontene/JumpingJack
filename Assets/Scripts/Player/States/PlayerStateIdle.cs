﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerState
{
    public PlayerStateIdle(Player p) : base(p) { }

    public override void OnUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Player.transform.Translate(Vector2.right * horizontal * Player.speed * Time.deltaTime);
        if (horizontal > 0 && Player.Direction == -1) Player.Direction = 1;
        else if (horizontal < 0 && Player.Direction == 1) Player.Direction = -1;

        Player.Animator.SetBool("Walking", horizontal != 0);

        float playerX = Player.transform.position.x;
        if(playerX >= Player.Stage.rightSide.position.x || playerX <= Player.Stage.leftSide.position.x)
        {
            ChangeState("WrapAround");
        }

        if (Input.GetButtonDown("Vertical"))
        {
            OnJumpButtonPressed();
        }

        if (Player.CheckFloorHole())
        {
            ChangeState("Falling");
        }

        if (Player.CheckHazardCollision())
        {
            ChangeState("Hazard");
        }
    }

    private void OnJumpButtonPressed()
    {
        RaycastHit2D hit = Physics2D.Raycast(Player.transform.position, Vector2.up, Player.jumpHeight, Player.holeLayer);
        if (hit)
        {
            ChangeState("Jumping");
        }
        else
        {
            ChangeState("Crashing");
        }
    }
}
