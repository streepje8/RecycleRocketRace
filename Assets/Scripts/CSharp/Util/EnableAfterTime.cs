using UnityEngine;

public class EnableAfterTime : MonoBehaviour
{
    public GameObject toActivate;
    public float timeBeforeActivation;

    private float timer = 0f;

    private void Start()
    {
        toActivate.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBeforeActivation)
        {
            toActivate.SetActive(true);
            enabled = false;
        }
    }
}