using UnityEngine;

public class AudioInputManager : MonoBehaviour
{
    CalibrationValues calibrationValues;

    [SerializeField] private AudioSource audioSource;
    // [SerializeField] float audioInputThreshold = 0.2f;
    
    ChickenInputManager chickenInputManager;

    private void Awake()
    {
        chickenInputManager = GetComponent<ChickenInputManager>();

    }

    private void Start()
    {
        KinectInputs kinectInputs = FindObjectOfType<KinectInputs>();

        if(kinectInputs != null) calibrationValues = kinectInputs.CalibrationValues;
    }

    // Update is called once per frame
    
    void Update()
    {
        if (Time.timeScale == 0) return;

        float loudness = AudioLoudnessDetection.GetLoudnessFromMicrophone();

        if (loudness > calibrationValues.ambientNoiseMaxValue + calibrationValues.ambientToCallNoiseDifference * calibrationValues.loundessPercentageToTriggerInput)
        {
            chickenInputManager.Call();
        }
    }

}