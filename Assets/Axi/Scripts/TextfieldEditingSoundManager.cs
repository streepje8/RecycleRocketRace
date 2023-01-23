using FMODUnity;
using TMPro;
using UnityEngine;

public class TextfieldEditingSoundManager : MonoBehaviour
{
    [SerializeField] private EventReference addLetterSound;
    [SerializeField] private EventReference removeLetterSound;
    private string textInput;
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    private void Start()
    {
        textInput = inputField.text;
    }

    public void OnValueChanged(string value)
    {
        if (textInput.Length > value.Length)
        {
            RuntimeManager.PlayOneShot(removeLetterSound);
        }
        
        if (textInput.Length < value.Length)
        {
            RuntimeManager.PlayOneShot(addLetterSound);
        }

        textInput = value;
    }
}
