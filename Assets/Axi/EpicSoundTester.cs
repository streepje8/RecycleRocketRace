using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class EpicSoundTester : MonoBehaviour
{
    private EventInstance eventInstance;

    public string fmodEventPath;
    public string fmodParameterName;
    [Range(0f, 1f)]
    public float parameterValue;
    
    void Start()
    {
        eventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        eventInstance.start();
    }

    void Update()
    {
        eventInstance.setParameterByName(fmodParameterName, parameterValue);
    }
}
