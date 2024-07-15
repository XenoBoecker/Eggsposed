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

        calibrationValues = FindObjectOfType<KinectInputs>().CalibrationValues;
    }

    // Update is called once per frame

    void Update()
    {
        float loudness = AudioLoudnessDetection.GetLoudnessFromMicrophone();

        if (loudness > calibrationValues.ambientNoiseMaxValue + calibrationValues.ambientToCallNoiseDifference * calibrationValues.loundessPercentageToTriggerInput)
        {
            chickenInputManager.Call();
        }
    }

}