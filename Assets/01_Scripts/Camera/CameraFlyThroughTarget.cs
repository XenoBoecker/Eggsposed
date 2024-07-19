using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlyThroughTarget : MonoBehaviour
{
    public float stoppingTime = 0.2f;

    public AnimationCurve flyPositionCurve;
    public AnimationCurve flyRotationCurve;

    public bool waitForInput;
}
