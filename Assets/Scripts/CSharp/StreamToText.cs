using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StreamToText : MonoBehaviour
{
    public Stream stream;
    public string prefix = "";
    public string suffix = "";
    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        stream.AddOutputStream<object>((val) => text.text = prefix + val + suffix);
    }
}