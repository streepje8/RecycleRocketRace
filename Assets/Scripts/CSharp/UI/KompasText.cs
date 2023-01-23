using TMPro;
using UnityEngine;

public class KompasText : MonoBehaviour
{
    public float height;

    public string text
    {
        set => tmpText.text = value;
        get => tmpText.text;
    }

    public TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }
}