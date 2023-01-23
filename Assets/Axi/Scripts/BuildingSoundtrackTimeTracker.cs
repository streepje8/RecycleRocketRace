using UnityEngine;

public class BuildingSoundtrackTimeTracker : MonoBehaviour
{
    private SoundController buildSoundtrack;
    private UITimer uiTimer;
    
    void Awake()
    {
        uiTimer = FindObjectOfType<UITimer>();
        buildSoundtrack = GetComponent<SoundController>();
    }

    void Update()
    {
        if (uiTimer.started)
            buildSoundtrack.SetParameter("Time", uiTimer.currentTime / uiTimer.time);
    }
}
