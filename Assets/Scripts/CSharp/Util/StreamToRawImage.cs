using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class StreamToRawImage : MonoBehaviour
{
    public Stream stream;
    private RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        stream.AddOutputStream<Texture2D>(value => { image.texture = value; });
    }
}