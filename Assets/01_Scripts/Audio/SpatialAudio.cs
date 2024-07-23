using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SpatialAudio : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0f, 1f)] private float maxPan = 1f;
    [SerializeField, Range(0f, 1f)] private float volumeBehindWall = 0.5f;
    private float stereoPan;
    private float baseVolume;

    [Header("Spatial Audio Values")]
    [SerializeField] private float minimumDistance = 5f;
    [SerializeField] private float maximumDistance = 15f;
    [SerializeField] private AnimationCurve volumeDropoff;

    private Transform headPos;
    private float distanceToPlayer;

    private Vector3 direction;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        baseVolume = audioSource.volume;

        headPos = Camera.main.transform;
    }

    private void Update()
    {
        direction = (headPos.position - transform.position);
        distanceToPlayer = (headPos.position - transform.position).magnitude;

        #region Pan
        //Returns the value for the stereo pan based on the player Head position relative to the Object
        stereoPan = (Vector3.Dot(headPos.right, direction.normalized) * -1f);
        stereoPan = Mathf.Clamp(stereoPan, -1, 1) * maxPan;

        audioSource.panStereo = stereoPan;
        #endregion

        #region Volume
        float normalizedDistance = Mathf.Clamp01((distanceToPlayer - minimumDistance) / (maximumDistance - minimumDistance));
        float volumeMultiplier = volumeDropoff.Evaluate(normalizedDistance);
        audioSource.volume = baseVolume * PlayerInSight() * volumeMultiplier;
        #endregion
    }

    float PlayerInSight()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maximumDistance))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                return 1f;
            }
        }
        Debug.DrawRay(transform.position, direction, Color.red);
        return volumeBehindWall;
    }
}