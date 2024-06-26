using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 2.4f, -3.5f);

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

        transform.rotation = target.rotation;

        transform.Rotate(Vector3.right, 20);
    }
}