using UnityEngine;

public class Instructions : MonoBehaviour
{
    public float speed = 10f;
    public float goalX = 0f;
    
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(goalX, transform.localPosition.y, transform.localPosition.z), speed * Time.deltaTime);
    }

    public void SetGoalX(float x) => goalX = x;
}
