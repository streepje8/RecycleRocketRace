using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CountdownScreen : MonoBehaviour
{
    public int startNumber = 3;
    private TMP_Text text;
    private float timer = 0f;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            startNumber--;
            text.text = startNumber.ToString();
            timer = 0f;
            if (startNumber <= 0) GameController.Instance.NextScreen();
        }
    }
}