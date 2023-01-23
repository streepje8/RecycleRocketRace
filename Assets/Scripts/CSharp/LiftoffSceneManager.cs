using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using SmoothShake;
using UnityEngine.Events;
using UnityEngine.VFX;

public class LiftoffSceneManager : MonoBehaviour
{
    public DetectYell screamer;
    public bool multiplyByCheerByMicVolume = true;
    public float cheerMultiplier = 100f;
    public Stream statsStream;
    public Stream heightStream;
    public Stream VFXStream;
    public Stream scoreStream;
    public float determendHeight = 0f;
    public float currentHeight = 0f;
    public float goalBMI = 1f;
    public PlayableDirector pd;
    public float jerk = 0f;
    public float heightModifier = 1f;
    private float accelleration = 0f;

    public bool IsFlying
    {
        get => currentHeight > 0 && currentHeight < determendHeight;
    }

    private bool hasTriggeredLiftOff = false;
    public UnityEvent liftoffEvent;
    public SmoothShake3D cameraShake;
    public SmoothShake2D knobShake; //knob in UI 
    public SmoothShake3D rocketShake; //Player object
    public SmoothShake3D rocketWiggle; //Extra shake player object
    public SmoothShake3D launchShake; //parent van camera voor launch
    public VisualEffect LaunchVFX;
    Vector3 VFXGroundPosition;
    public Vector3 LaunchSpeedMovementForVFX;
    Vector3 vfxmovementspeedsaver;
    public GameObject scoreSceneObject;

    private bool hasSaved = true;

    private void Awake()
    {
        scoreSceneObject.SetActive(false);
        vfxmovementspeedsaver = LaunchSpeedMovementForVFX;
        VFXGroundPosition = new Vector3(0, 0, 0);
        statsStream.AddOutputStream<RocketStats>((stats) =>
        {
            float BMI = (stats.weight * 5000) / (float)(Mathf.Clamp(stats.height,1,1920));
            float inefficiency = Mathf.Abs(goalBMI - BMI);
            determendHeight = stats.height * 2 * (1/inefficiency) * heightModifier;
        });
    }

    void Start()
    {
        LaunchVFX.Play();
        pd.time = 0f;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasTriggeredLiftOff)
        {
            LiftOff();
            liftoffEvent?.Invoke();
            hasTriggeredLiftOff = true;
        }
        if (currentHeight >= determendHeight - 200f)
        {
            LaunchVFX.Stop();
        }

        if (currentHeight >= determendHeight)
        {
            knobShake.fadeOutCurrentShake(5f);
            rocketShake.fadeOutCurrentShake(2f);
            rocketWiggle.fadeOutCurrentShake(2f);

            VFXStream.StreamData(currentHeight);
            //isFlying = false;
            scoreSceneObject.SetActive(true);
            if (!hasSaved)
            {
                scoreStream.StreamData(determendHeight);
                hasSaved = true;
            }
            //GameController.Instance.NextScreen();
        }

        pd.time = (currentHeight / 4000f) * 60f;
        
        if (IsFlying)
        {
            accelleration += jerk * Time.deltaTime;
            currentHeight += accelleration;
            VFXGroundPosition += LaunchSpeedMovementForVFX * Time.deltaTime;
            LaunchVFX.SetVector3("PlanePosition", (VFXGroundPosition));
            if (screamer.yelling)
            {
                determendHeight += cheerMultiplier * Time.deltaTime *
                                   (multiplyByCheerByMicVolume ? screamer.microphoneLevel : 1f);
            }
        }

        heightStream.StreamData(currentHeight);
    }

    public void LiftOff()
    {
        //isFlying = true;
        accelleration += jerk * Time.deltaTime;
        currentHeight += accelleration;
        cameraShake.StartShake();
        hasSaved = false;
        StartCoroutine(LaunchSequence());
        knobShake.StartShake();
        rocketShake.StartShake();
        rocketWiggle.StartShake();
        launchShake.StartShake();
        //LaunchVFX.Play();
    }

    IEnumerator LaunchSequence()
    {
        LaunchSpeedMovementForVFX = new Vector3(0, LaunchSpeedMovementForVFX.y / 2, 0);
        yield return new WaitForSeconds(0.5f);
        LaunchSpeedMovementForVFX = new Vector3(0, vfxmovementspeedsaver.y, 0);
        yield return null;
    }
}
