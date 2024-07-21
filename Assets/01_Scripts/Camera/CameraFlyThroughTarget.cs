using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlyThroughTarget : MonoBehaviour
{
    public float stoppingTime = 0.2f;

    public AnimationCurve flyPositionCurve;
    public AnimationCurve flyRotationCurve;

    public bool waitForInput;

    public string tutorialText;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, Camera.main.farClipPlane, Camera.main.nearClipPlane, Camera.main.aspect);
    }
}
