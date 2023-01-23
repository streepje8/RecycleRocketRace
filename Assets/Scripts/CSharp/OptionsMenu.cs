using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public bool inOptions = false;

    void Update()
    {
        if (inOptions)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-180, -90, 90),
                4f * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-90, -90, 90),
                4f * Time.deltaTime);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                child.GetChild(j).rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void SetDisplayOptions(bool b) => inOptions = b;
}