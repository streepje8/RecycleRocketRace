using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class Webcam : MonoBehaviour
{
    public TMP_Dropdown uiVisual;
    public Stream webcamStream;
    public Material backgroundRemovalShader;
    public KeyCode calibrationKey = KeyCode.LeftShift;
    private WebCamTexture texture;

    void Start()
    {
        foreach (var webCamDevice in WebCamTexture.devices)
        {
            if (uiVisual) uiVisual.options.Add(new TMP_Dropdown.OptionData(webCamDevice.name));
        }

        string name = PlayerPrefs.GetString("webcamName", "NOTFOUND");
        if (!name.Equals("NOTFOUND", StringComparison.OrdinalIgnoreCase))
        {
            if (WebCamTexture.devices.Where(x => x.name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList()
                    .Count > 0)
            {
                texture = new WebCamTexture(name);
                uiVisual.SetValueWithoutNotify(uiVisual.options.IndexOf(uiVisual.options
                    .Where(x => x.text.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList()[0]));
            }
            else
            {
                texture = new WebCamTexture();
            }
        }
        else
        {
            texture = new WebCamTexture();
        }

        texture.Play();
        backgroundRemovalShader.mainTexture = texture;
        webcamStream.StreamData(texture);
    }

    public void UpdateDevice()
    {
        if (uiVisual)
        {
            string name = uiVisual.options[uiVisual.value].text;
            if (!name.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                texture.Stop();
                texture.deviceName = name;
                PlayerPrefs.SetString("webcamName", name);
                texture.Play();
            }
        }
    }

    public void Calibrate()
    {
        Texture tex = new Texture2D(backgroundRemovalShader.mainTexture.width,
            backgroundRemovalShader.mainTexture.height, TextureFormat.RGBA32,
            backgroundRemovalShader.mainTexture.mipmapCount, false);
        Graphics.CopyTexture(backgroundRemovalShader.mainTexture, tex);
        backgroundRemovalShader.SetTexture("_Comparison", tex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(calibrationKey)) Calibrate();
    }

    private void OnDestroy()
    {
        texture.Stop();
    }

    private void OnApplicationQuit()
    {
        texture.Stop();
    }
}