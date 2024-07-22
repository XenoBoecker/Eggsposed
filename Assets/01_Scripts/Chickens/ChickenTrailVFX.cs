using UnityEngine;

public class ChickenTrailVFX : MonoBehaviour
{
    ChickenStateTracker cst;

    [SerializeField] TrailRenderer leftTrail, rightTrail;

    // Start is called before the first frame update
    void Start()
    {
        cst = GetComponent<ChickenStateTracker>();

        cst.OnStartGlide += StartTrailEffect;
        cst.OnStopGlide += StopTrailEffect;

        StopTrailEffect();
    }

    public void StartTrailEffect()
    {
        leftTrail.enabled = true;
        rightTrail.enabled = true;
    }

    public void StopTrailEffect()
    {
        leftTrail.enabled = false;
        rightTrail.enabled = false;
    }
}
