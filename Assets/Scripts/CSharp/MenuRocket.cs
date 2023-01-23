using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class MenuRocket : MonoBehaviour
{
    public float lifetime = 20f;

    private float speed = 3;
    private SpriteRenderer sRenderer;
    private float timer = 0f;
    private float timerGoal = 0.5f;
    private Vector3 direction = Vector3.zero;

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);
        timer += Time.deltaTime;
        if (timer >= timerGoal)
        {
            direction = Quaternion.Euler(0, 0, Random.Range(0,11) > 5 ? -10 : 10) * direction;
            timerGoal = Random.Range(0.2f, 2f);
            timer = 0;
        }
        transform.rotation =
            Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.left, direction),
                2f * Time.deltaTime);
        transform.position += direction * (-speed * Time.deltaTime);
    }

    public void SetTexture(Texture2D rocket)
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.sprite = Sprite.Create(rocket, new Rect(0.0f, 0.0f, rocket.width, rocket.height),
            new Vector2(0.5f, 0.5f),
            100);
        transform.position =
            new Vector3(50, 0, 110) + Quaternion.Euler(0, 0, Random.Range(0, 360)) * (Vector3.up * 100f);
        direction = (transform.position - new Vector3(50, 0, 110)).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, direction);
        speed = Random.Range(40, 60);
    }
}