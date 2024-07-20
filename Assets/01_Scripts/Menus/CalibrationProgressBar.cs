using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationProgressBar : MonoBehaviour
{
    KinectCalibration calibration;

    [SerializeField] Image fillImage;


    float totalCount;

    // Start is called before the first frame update
    void Start()
    {
        calibration = FindObjectOfType<KinectCalibration>();

        totalCount = calibration.CalibrationQueueSize;
    }

    // Update is called once per frame
    void Update()
    {
        float currentCount = calibration.CalibrationQueueCounter;

        fillImage.fillAmount = currentCount / totalCount;
    }
}
