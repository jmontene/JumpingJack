using System;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : StageObject
{
    [Header("Sprites")]

    public Sprite idleSprite1;
    public Sprite idleSprite2;
    public SpriteRenderer spriteRenderer;

    public void SwapSprite()
    {
        spriteRenderer.sprite = spriteRenderer.sprite == idleSprite1 ? idleSprite2 : idleSprite1;
    }
}