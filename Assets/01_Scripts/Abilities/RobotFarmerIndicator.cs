using System;
using TMPro;
using UnityEngine;

public class RobotFarmerIndicator : MonoBehaviour
{
    Transform farmer;


    [SerializeField] ChickenData robotChickenData;
    Chicken chicken;


    [SerializeField] GameObject distanceCanvas;
    [SerializeField] TMP_Text farmerDistanceText;
    [SerializeField] Transform farmerHeadPointer, farmerBodyPointer;

    // [SerializeField] float sonarSoundCD;
    // float sonarSoundTimer;

    AudioSource sonarAudioSource;

    private void Start()
    {
        farmer = FindObjectOfType<FarmerAutoInput>().transform;

        sonarAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //sonarSoundTimer += Time.deltaTime;
        //
        //if (sonarSoundTimer > sonarSoundCD)
        //{
        //    SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.sonarBeeping, sonarAudioSource);
        //    sonarSoundTimer = 0;
        //}

        // farmerDistanceText.text = ((int)Vector3.Distance(transform.position, farmer.position)).ToString();

        LookAtOnYAxis(farmerHeadPointer, farmer);
        LookAtOnYAxis(farmerBodyPointer, farmer);

        if(!chicken.IsControlledByPlayer) SoundManager.Instance.EndLoopingSound(sonarAudioSource);
    }
    void LookAtOnYAxis(Transform pointer, Transform target)
    {
        Vector3 direction = target.position - pointer.position;
        direction.y = 0; // Keep only the horizontal direction

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 currentRotation = pointer.rotation.eulerAngles;
            pointer.rotation = Quaternion.Euler(currentRotation.x, targetRotation.eulerAngles.y, currentRotation.z);
        }
    }

    public void Activate(Chicken chicken)
    {
        this.chicken = chicken;
        SoundManager.Instance.StartLoopingSound(SoundManager.Instance.chickenSFX.sonarBeeping, sonarAudioSource);
        distanceCanvas.SetActive(true);
    }
}
