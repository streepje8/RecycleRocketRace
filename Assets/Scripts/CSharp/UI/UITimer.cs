using System;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public int time = 300;
    public bool started = false;
    private TMP_Text displayText;
    public float currentTime = 0f;

    private bool calibrated = false;

    private void Start()
    {
        displayText = GetComponent<TMP_Text>();
        TimeSpan t = TimeSpan.FromSeconds(time - currentTime);
        displayText.text = t.ToString(@"mm\:ss");
        calibrated = false;
    }

    void Update()
    {
        if (started) currentTime += Time.deltaTime;
        if (!calibrated)
        {
            GameController.Instance.Webcam.Calibrate();
            calibrated = true;
        }
        TimeSpan t = TimeSpan.FromSeconds(time - currentTime);
        displayText.text = t.ToString(@"mm\:ss");
        
        if (Input.GetKeyDown(KeyCode.Space)) currentTime += 60;
        if (currentTime >= time && started) GameController.Instance.NextScreen();
    }

    public void StartTimer() => started = true;
}