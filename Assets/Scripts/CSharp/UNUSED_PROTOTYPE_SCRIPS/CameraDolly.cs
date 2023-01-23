using UnityEngine;

public class CameraDolly : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 goal = target.localPosition;
        goal.y = target.transform.position.y;
        transform.localPosition = Vector3.Lerp(transform.localPosition, goal, 10f * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up),
            10f * Time.deltaTime);
    }
}