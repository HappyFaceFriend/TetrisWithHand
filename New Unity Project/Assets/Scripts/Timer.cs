using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float targetTime;
    public float elapsedTime;
    public bool isOn;
    public bool isOver;
    public Timer(float targetTime, bool isOn = false)
    {
        elapsedTime = 0f;
        this.targetTime = targetTime;
        this.isOn = isOn;
        isOver = false;
    }
    public void Reset()
    {
        elapsedTime = 0f;
        isOn = false;
        isOver = false;
    }
    public void Start(float targetTime = -1)
    {
        if (targetTime != -1)
            this.targetTime = targetTime;
        isOn = true;
        isOver = false;
    }
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= targetTime)
            isOver = true;
    }
    public bool Check()
    {
        return isOver;
    }
    
}
