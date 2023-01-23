using System;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private bool started = false;
    private float startVal = 0f;
    private float endVal = 0f;
    private bool useEndVal = false;
    private float val = 0f;
    private bool isPaused = false;
    private List<Action> listeners = new List<Action>();

    public float EndValue
    {
        get => endVal;
        set
        {
            useEndVal = true;
            endVal = value;
        }
    }
    
    public bool IsStarted
    {
        get => started;
    }
    public float Value
    {
        get => val;
    }

    public void UnPause() => isPaused = false;

    public void Pause() => isPaused = true;

    public void Start()
    {
        TimerManager.CheckInitialized();
        if (!started)
        {
            val = startVal;
            started = true;
        }
    }

    public void Stop()
    {
        if (started)
        {
            val = startVal;
            started = false;
        }
    }

    public void Update()
    {
        if (started && !isPaused) val += Time.deltaTime;
        if (useEndVal)
        {
            if (val > endVal)
            {
                listeners.ForEach(x => x.Invoke());
                Stop();
            }
        }
    }

    public void DiscardOnEnd()
    {
        listeners.Add(() =>
        {
            TimerManager.DiscardTimer(this);
        });
    }

        public void AddListener(Action a)
    {
        listeners.Add(a);
    }
    
    public void Set(float value)
    {
        val = value;
    }

    public void Reset()
    {
        Stop();
        Start();
    }
}
