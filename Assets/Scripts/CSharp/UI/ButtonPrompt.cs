using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    public KeyCode key;
    public List<Texture2D> sprites = new List<Texture2D>();
    public float fps = 2;

    private int index = 0;
    private RawImage img;
    private float timer = 0f;

    void Start()
    {
        img = GetComponent<RawImage>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1 / fps)
        {
            img.texture = sprites[index];
            index++;
            if (index >= sprites.Count) index = 0;
            timer = 0f;
        }

        if (Input.GetKeyDown(key)) gameObject.SetActive(false);
    }
}