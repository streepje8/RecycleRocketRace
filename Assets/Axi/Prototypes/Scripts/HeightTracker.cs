using UnityEngine;
using TMPro;

public class HeightTracker : MonoBehaviour
{
    [SerializeField] private Transform trackingObject;
    [SerializeField] TMP_Text currentHeightText;
    [SerializeField] TMP_Text highestHeightText;
    private float maxHeight;
    
    void Update()
    {
        maxHeight = Mathf.Max(maxHeight, trackingObject.position.y);
        highestHeightText.text = "Highest: " + maxHeight.ToString("F0");
        currentHeightText.text = "Current: " + trackingObject.position.y.ToString("F0");
    }
}
