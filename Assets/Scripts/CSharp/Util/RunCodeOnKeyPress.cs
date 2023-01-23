using UnityEngine;
using UnityEngine.Events;

public class RunCodeOnKeyPress : MonoBehaviour
{
    public KeyCode key;
    public UnityEvent e;

    void Update()
    {
        if (Input.GetKeyDown(key))
            e.Invoke();
    }
}