using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    private float timer;
    private TextMeshProUGUI timerText;
    private bool timing;
    public float timerDuration = 3f;
    [SerializeField] UnityEvent timerCompleteEvent;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (timing)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Floor(timer).ToString("F0");
            
            if (timer < 1f)
            {
                timing = false;
                timer = 0f;
                timerCompleteEvent?.Invoke();
                timerText.text = "LAUNCH";
                Destroy(timerText.gameObject, 1f);
            }
        }
    }

    public void StartTimer()
    {
        timing = true;
        timer = timerDuration;
    }
}
