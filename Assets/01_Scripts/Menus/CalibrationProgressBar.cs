using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationProgressBar : MonoBehaviour
{
    KinectCalibration calibration;

    [SerializeField] Image fillImage;
    // Start is called before the first frame update
    void Start()
    {
        calibration = FindObjectOfType<KinectCalibration>();
    }

    // Update is called once per frame
    void Update()
    {
        fillImage.fillAmount = calibration.CalibrationQueueCounter / calibration.CalibrationQueueSize;
    }
}
