using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float velocityThreshold;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (rb.velocity.magnitude > velocityThreshold)
        {
            transform.LookAt(rb.velocity + new Vector2(transform.position.x, transform.position.y));
            transform.eulerAngles += rotationOffset;
        }
    }
}
