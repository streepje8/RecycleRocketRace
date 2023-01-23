using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RocketLaunch : MonoBehaviour
{
    public float speed;
    public float fuel;
    public Vector2 boostTimeRange = Vector2.zero;
    [Range(0f, 1f)] public float spendFuelChance = 1f;
    [SerializeField] public float spendFuelTimeout = 3f;
    [SerializeField] private ParticleSystem boostParticles;
    private Timer spendFuelTimeoutTimer = TimerManager.CreateTimer();
    private bool isUsingFuel = false;

    public bool IsUsingFuel
    {
        get => isUsingFuel;
        set
        {
            isUsingFuel = value;
            ActivateParticles(IsUsingFuel);
        }
    }

    Rigidbody2D rb;
    [SerializeField] ParticleSystem usingFuelParticles;
    [SerializeField] float usingFuelParticlesRateOverTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ActivateParticles(IsUsingFuel);
        TriggerBoost();
        spendFuelTimeoutTimer.AddListener(TriggerBoost);
        spendFuelTimeoutTimer.EndValue = spendFuelTimeout;
    }

    void Update()
    {
        if (fuel <= 0f)
        {
            IsUsingFuel = false;
            return;
        }
        
        if (!IsUsingFuel)
            return;

        fuel -= Time.deltaTime;
        rb.AddForce(transform.up * (Time.deltaTime * speed * 100f));
    }

    private void FixedUpdate()
    {
        if (IsUsingFuel || fuel <= 0f)
            return;

        if (Random.value < spendFuelChance)
        {
            spendFuelTimeoutTimer.Stop();
            TriggerBoost();
        }
    }

    private void TriggerBoost()
    {
        IsUsingFuel = true;

        rb.AddForce(transform.up * (speed * 10f), ForceMode2D.Impulse);
        Instantiate(boostParticles, transform.position, Quaternion.identity);
        
        Timer boostTimer = TimerManager.CreateTimer();
        boostTimer.EndValue = 2f;
        boostTimer.AddListener(() =>
        {
            IsUsingFuel = false;
            print("timer ended");
        });
        boostTimer.DiscardOnEnd();
        boostTimer.Start();
        spendFuelTimeoutTimer.Reset();
    }

    private void ActivateParticles(bool usingFuel)
    {
        var em = usingFuelParticles.emission;
        em.rateOverTime = usingFuel ? usingFuelParticlesRateOverTime : 0f;
    }
}