using UnityEngine;

public class RocketCameraTracker : MonoBehaviour
{
    [SerializeField] Rigidbody2D rocketRb;
    [SerializeField] private Transform rocketTracker;
    [SerializeField] private float positionShakingIntensity;
    [SerializeField] private float rotationShakingIntensity;
    [SerializeField] private float cameraDistanceMultiplier;
    [SerializeField] private float xPositionFrequency;
    [SerializeField] private float yPositionFrequency;
    [SerializeField] private float zPositionFrequency;
    [SerializeField] private float xRotationFrequency;
    [SerializeField] private float yRotationFrequency;
    [SerializeField] private float zRotationFrequency;

    void Update()
    {
        float shakeIntensity = rocketRb.velocity.magnitude;
        float x = Mathf.Sin(Time.time * xPositionFrequency) * shakeIntensity * positionShakingIntensity / 100f;
        float y = Mathf.Sin(Time.time * yPositionFrequency) * shakeIntensity * positionShakingIntensity / 100f;
        float z = Mathf.Sin(Time.time * zPositionFrequency) * shakeIntensity * positionShakingIntensity / 100f;
        //rocketTracker.localPosition = new Vector3(x, y, z);

        float rotShakeX = Mathf.Sin(Time.time * xRotationFrequency) * shakeIntensity * rotationShakingIntensity / 100f;
        float rotShakeY = Mathf.Sin(Time.time * yRotationFrequency) * shakeIntensity * rotationShakingIntensity / 100f;
        float rotShakeZ = Mathf.Sin(Time.time * zRotationFrequency) * shakeIntensity * rotationShakingIntensity / 100f;
        rocketTracker.localRotation = Quaternion.Euler(rotShakeX, rotShakeY, rotShakeZ);

        //transform.localPosition = Vector3.forward * (-18f - rocketRb.velocity.magnitude * cameraDistanceMultiplier);
    }
}
