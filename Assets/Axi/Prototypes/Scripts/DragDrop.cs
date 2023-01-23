using UnityEngine;

public class DragDrop : MonoBehaviour
{
    bool mouseOver = false;
    public float intensity = 5f;
    private Vector3 currentPosition;
    private Vector3 lastPosition;

    private void Update()
    {
        currentPosition = Input.mousePosition;
        Vector3 deltaPosition = currentPosition - lastPosition;
        lastPosition = currentPosition;

        if (mouseOver)
            transform.position += deltaPosition / 100f * intensity;
    }
    private void OnMouseDown()
    {
        mouseOver = true;
    }

    private void OnMouseUp()
    {
        mouseOver = false;
    }
}