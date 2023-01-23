using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SoundController : MonoBehaviour
{
    public EventReference eventReference;
    private EventInstance instance;

    private void Awake()
    {
        instance = RuntimeManager.CreateInstance(eventReference);
    }

    private void OnEnable()
    {
        Start();
    }

    [ContextMenu("Stop Song")]
    public void Stop()
    {
        instance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    [ContextMenu("Start Song")]
    public void Start()
    {
        instance.getPlaybackState(out PLAYBACK_STATE state);

        if (state != PLAYBACK_STATE.PLAYING)
            instance.start();
    }

    public void SetParameter(string parameterName, float parameterValue)
    {
        instance.setParameterByName(parameterName, parameterValue);
    }

    private void OnDisable()
    {
        instance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}