using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class StreamToSprite : MonoBehaviour
{
    public Stream stream;
    private SpriteRenderer rendererToSet;
    public float ppu = 1000f;

    private void Awake()
    {
        rendererToSet = GetComponent<SpriteRenderer>();
        stream.AddOutputStream<Texture2D>((value) =>
        {
            rendererToSet.sprite = Sprite.Create(value, new Rect(0.0f, 0.0f, value.width, value.height),
                new Vector2(0.5f, 0.5f), ppu);
        });
    }
}