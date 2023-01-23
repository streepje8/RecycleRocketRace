using UnityEngine;

public class GrabCameraTexture : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 200, 200, 200), "Change"))
        {
            GetComponent<Renderer>().material.mainTexture = FindObjectOfType<CameraCapturer>().TakeSnapshot();
        }
    }
}
