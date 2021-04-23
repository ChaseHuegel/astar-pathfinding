using System;
using UnityEngine;

public class Timer
{
    private float currentTime = 0.0f;
    private float maxTime = 1.0f;

    private bool autoReset = false;

    public Timer(float _maxTime, bool _doesReset = false)
    {
        maxTime = _maxTime;
        autoReset = _doesReset;
    }

    public bool Tick()
    {
        if (currentTime >= maxTime)
        {
            if (autoReset == true)
            {
                currentTime = 0.0f;
            }
        
            return true;    //  The timer has finished
        }
        
        currentTime += Time.deltaTime;

        return false;   //  Timer is not finished
    }

    public float GetTimeLeft()
    {
        return maxTime - currentTime;
    }

    public float GetTime()
    {
        return currentTime;
    }

	public void Reset(float _maxTime = 0.0f)
	{
		currentTime = 0.0f;
		
		if (_maxTime != 0.0f) { maxTime = _maxTime; }
	}

	public bool IsDone()
	{
		return (currentTime >= maxTime);
	}
}
