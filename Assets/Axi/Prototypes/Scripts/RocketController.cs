using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.eulerAngles += Vector3.forward * speed * horizontalInput * Time.deltaTime;
    }
}
