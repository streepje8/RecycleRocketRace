using UnityEngine;

public class UpScrollTest : MonoBehaviour
{
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
    }
}
