using UnityEngine;
using FMODUnity;

public class FModSoundEventTest : MonoBehaviour
{
    [SerializeField] StudioEventEmitter studioEventEmitter;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 20, 20), "funky sound alert!"))
            studioEventEmitter.Play();
    }
}