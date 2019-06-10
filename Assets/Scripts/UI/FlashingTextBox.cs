using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashingTextBox : MonoBehaviour
{
    public float flashTime = 0.1f;
    public Color color1;
    public Color color2;

    protected Image img;
    protected bool colorSwitch;
    protected float timer;

    void Awake()
    {
        img = GetComponent<Image>();
        colorSwitch = true;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= flashTime)
        {
            timer = 0f;
            img.color = (colorSwitch ? color1 : color2);
            colorSwitch = !colorSwitch;
        }
        
    }
}
