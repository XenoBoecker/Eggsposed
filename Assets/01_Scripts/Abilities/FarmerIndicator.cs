using System;
using TMPro;
using UnityEngine;

public class FarmerIndicator : MonoBehaviour
{
    Transform farmer;

    [SerializeField] TMP_Text farmerDistanceText;
    [SerializeField] Transform farmerPointer;

    [SerializeField] float sonarSoundCD;
    float sonarSoundTimer;

    AudioSource sonarAudioSource;

    private void Start()
    {
        farmer = FindObjectOfType<FarmerAutoInput>().transform;

        sonarAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        sonarSoundTimer += Time.deltaTime;

        if (sonarSoundTimer > sonarSoundCD)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.sonarBeeping, sonarAudioSource);
            sonarSoundTimer = 0;
        }

        farmerDistanceText.text = ((int)Vector3.Distance(transform.position, farmer.position)).ToString();

        LookAtOnYAxis(farmerPointer, farmer);
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
}
