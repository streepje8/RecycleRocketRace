using System.Collections.Generic;
using UnityEngine;

public class TimerManagerInjector : MonoBehaviour
{ 
    private void Update() => TimerManager.Update();
}

public class TimerManager
{
    private static TimerManager instance = new TimerManager();
    private List<Timer> timers = new List<Timer>();
    private GameObject timerOBJ;

    public static Timer CreateTimer()
    {
        Timer t = new Timer();
        instance.timers.Add(t);
        return t;
    }

    public static void CheckInitialized()
    {
        if (!instance.timerOBJ)
        {
            instance.timerOBJ = new GameObject("TimerManager");
            TimerManagerInjector tmi = instance.timerOBJ.AddComponent<TimerManagerInjector>();
        }
    }

    public static void Update()
    {
        for (int i = instance.timers.Count - 1; i >= 0; i--)
            instance.timers[i].Update();
    }

    public static void DiscardTimer(Timer timer)
    {
        if (instance.timers.Contains(timer)) instance.timers.Remove(timer);
    }
}
