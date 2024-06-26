using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 offset;

    void Awake()
    {
        offset = transform.position - transform.parent.position;
    }

    private void Start()
    {
        GameManager.Instance.OnSpawnChicken += TargetNewChicken;
    }

    void TargetNewChicken()
    {
        SetTarget(GameManager.Instance.Player.transform);
    }

    public void SetTarget(Transform target)
    {
        transform.parent = target;

        transform.position = target.position + target.forward * offset.z + target.up * offset.y;
    }
}