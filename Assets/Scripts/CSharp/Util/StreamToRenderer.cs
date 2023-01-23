using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class StreamToRenderer : MonoBehaviour
{
    public Stream stream;
    private Renderer rendererToSet;

    private void Awake()
    {
        rendererToSet = GetComponent<Renderer>();
        stream.AddOutputStream<Texture2D>((value) => { rendererToSet.material.mainTexture = value; });
    }
}