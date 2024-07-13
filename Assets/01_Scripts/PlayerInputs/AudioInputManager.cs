using UnityEngine;

public class AudioInputManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] float audioInputThreshold = 0.2f;
    
    ChickenInputManager chickenInputManager;

    private void Awake()
    {
        chickenInputManager = GetComponent<ChickenInputManager>();
    }

    // Update is called once per frame

    void Update()
    {
        float loudness = AudioLoudnessDetection.GetLoudnessFromMicrophone();
        

        if (loudness > audioInputThreshold)
        {
            chickenInputManager.Call();
        }
    }

}