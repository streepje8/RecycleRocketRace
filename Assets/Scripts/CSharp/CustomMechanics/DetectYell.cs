using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SmoothShake;

//Technically speaking MIC one will be used by default, which is your default mic in windows
public class DetectYell : MonoBehaviour
{
    [Tooltip("The sample range.")] int sampleRange = 128;

    [Tooltip("The dropdown UI.")] public TMP_Dropdown dropdown;

    [Tooltip("The currently selected microphone.")]
    public string selectedMic;

    [Tooltip("Used for adjusting the minimum yelling volume.")]
    public Slider slider;

    [Tooltip("Used to represent the current audio volume.")]
    public Slider currentVolume;

    [Tooltip("Shake when loud volume.")] public SmoothShake3D shake;

    //Used for scanning audio
    AudioClip microphoneAudio;

    [Tooltip("Interval of seconds of when it gets an audio snippet.")]
    public int sampleLength = 1;

    [Tooltip("The current microphone level.")]
    public float microphoneLevel;

    [Tooltip("Amplifies the microphone input number.")]
    public float amplifier = 100;

    [Tooltip("The volume required to reach for cheering, this is additional from the room volume.")]
    public float cheeringVolume = 1f;

    [Tooltip("The volume noise of the background.")]
    public float roomVolume;

    [Tooltip("Whether a microphone is present.")]
    public bool microphone = true;

    [Tooltip("Whether a yell has been detected.")]
    public bool yelling = false;

    [Tooltip("How many samples it takes when getting the average of the background noise.")]
    public int roomVolumeSamples = 5;

    List<string> dropOptions = new List<string>();

    //Game loop
    private void Update()
    {
        FetchMic();

        //Fallback when mic unplugs/crashes
        if (microphoneAudio == null)
        {
            SelectMic(0);
        }

        //Checking compared to default room value
        yelling = microphoneLevel > roomVolume + cheeringVolume;
        //Passing the current volume
        currentVolume.value = microphoneLevel;

        //Quit problem solving for a screenshake
        if (yelling && GameController.Instance.liftoffManager.IsFlying)
        {
            shake.StartShake();
        }
        else
        {
            shake.StopShake();
        }
    }


    IEnumerator FetchVolume()
    {
        List<float> averagerer = new List<float>();

        Debug.Log("Starting to create " + roomVolumeSamples + " volume samples..");
        for (int i = 0; i < roomVolumeSamples; i++)
        {
            averagerer.Add(microphoneLevel);
            print(microphoneLevel);
            Debug.Log("Done with sample : " + (i + 1));
            yield return new WaitForSeconds(1);
        }

        roomVolume = averagerer.Average();
        Debug.Log("Finished creating samples, average of " + roomVolume);
    }

    public void SetThreshold(float value)
    {
        PlayerPrefs.SetFloat("CheeringVolume", value);
        cheeringVolume = value;
    }

    public void CallibrateRoomVolume()
    {
        StartCoroutine(FetchVolume());
    }

    //Fetching the mic data
    void FetchMic()
    {
        float[] spectrum = new float[sampleRange];
        microphoneAudio.GetData(spectrum, sampleRange);

        // Getting a peak on the last 128 samples
        float levelMax = 0;
        for (int i = 0; i < sampleRange; i++)
        {
            float wavePeak = spectrum[i] * spectrum[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        //Pass the general loudness
        microphoneLevel = Mathf.Sqrt(levelMax * amplifier);
    }

    //Startup sequence and picking default mic
    void Start()
    {
        cheeringVolume = PlayerPrefs.GetFloat("CheeringVolume", 1);
        RegfreshList();
        slider.value = cheeringVolume;
    }

    //Get a new list of current microphones
    public void RegfreshList()
    {
        //Clear the old options of the Dropdown menu and list
        dropdown.ClearOptions();
        dropOptions.Clear();

        //Check what we're working with
        foreach (var device in Microphone.devices)
        {
            dropOptions.Add(device);
        }

        //Add the options created in the List above
        dropdown.AddOptions(dropOptions);

        SelectMic(PlayerPrefs.GetInt("Microphone", 0));
    }

    //When the application quits, gameobject gets removed, making sure it's closing properly.
    private void OnDisable()
    {
        turnOffMic();
    }

    //Used for selecting a different microphone
    public void SelectMic(int mic)
    {
        //Clear current mic
        turnOffMic();

        //Check if we can change to the next microphone
        if (dropOptions.Count > 0 && dropOptions[mic] != null) selectedMic = dropOptions[mic];

        //Otherwise revert back to default mic
        else if (dropOptions.Count > 0 && dropOptions[0] != null) selectedMic = dropOptions[0];

        //Finally, if neither work, throw an error
        else
        {
            NoMic();
            return;
        }

        //Storing settings
        dropdown.value = dropOptions.IndexOf(selectedMic);
        PlayerPrefs.SetInt("Microphone", dropOptions.IndexOf(selectedMic));

        //Let others know there's a microphone
        microphone = true;

        //Start fetching the sample for 1 second
        microphoneAudio = Microphone.Start(selectedMic, true, sampleLength, 44100);
    }

    //When there's no microphone
    void NoMic()
    {
        Debug.LogError("No usable microphones detected! " +
                       "\nCheering will be disabled for this session until a microphone has been connected and selected.");
        microphone = false;

        //Clear the old options of the Dropdown menu and list
        dropdown.ClearOptions();
        dropOptions.Clear();

        //add default message when no mic is found
        List<string> defaultText = new List<string> { "No microphone detected..." };
        dropdown.AddOptions(defaultText);
        dropdown.value = 0;

        this.gameObject.SetActive(false);
    }

    //Turning off the current microphone
    void turnOffMic()
    {
        microphoneAudio = null;
        if (selectedMic == null)
            Microphone.End(dropOptions[0].ToString());
        else
            Microphone.End(selectedMic);
    }
}