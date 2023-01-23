using UnityEngine;

public class CameraCapturer : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public string webCamName;
    [SerializeField] Renderer test;

    void Awake()
    {
        webCamTexture = new WebCamTexture(webCamName);
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            WebCamDevice device = WebCamTexture.devices[i];
            print(device.name);
        }
        webCamTexture.Play();
    }

    void Update()
    {
        test.material.mainTexture = webCamTexture; 
    }

    public Texture2D TakeSnapshot()
    {
        Texture2D snapShot = new Texture2D(webCamTexture.width, webCamTexture.height);
        snapShot.SetPixels(webCamTexture.GetPixels());
        snapShot.Apply();

        return snapShot;
    }
}
