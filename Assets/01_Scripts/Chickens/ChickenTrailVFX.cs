using UnityEngine;

public class ChickenTrailVFX : MonoBehaviour
{
    ChickenStateTracker cst;

    [SerializeField] TrailRenderer leftTrail, rightTrail;

    // Start is called before the first frame update
    void Start()
    {
        cst = GetComponent<ChickenStateTracker>();

        cst.OnStartGlide += StartGlide;
        cst.OnStopGlide += StopGlide;

        StopGlide();
    }

    private void StartGlide()
    {
        leftTrail.enabled = true;
        rightTrail.enabled = true;
    }

    private void StopGlide()
    {
        leftTrail.enabled = false;
        rightTrail.enabled = false;
    }
}
