using UnityEngine;

public class JumpInOnEnable : MonoBehaviour
{
    public Vector3 goalPosition = Vector3.zero;
    private bool canJump = false;

    private void OnEnable()
    {
        canJump = true;
    }

    void Update()
    {
        if (canJump)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, goalPosition, 4f * Time.deltaTime);
        }
    }
}