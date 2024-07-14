using UnityEngine;
using UnityEngine.UI;

public class PictogramAnimator : MonoBehaviour
{
    KinectCalibration calibrator;


    [SerializeField] Image pictogramImage;


    [SerializeField] float frameRate = 3;

    [SerializeField] Sprite[] standSprites;
    [SerializeField] Sprite[] headForwardSprites;
    [SerializeField] Sprite[] rotLeftSprites;
    [SerializeField] Sprite[] rotRightSprites;
    [SerializeField] Sprite[] jumpSprites;
    [SerializeField] Sprite[] glideSprites;
    [SerializeField] Sprite[] squatSprites;
    [SerializeField] Sprite[] hatchSprites;
    [SerializeField] Sprite[] dropEggSprites;
    [SerializeField] Sprite[] dropEggTutorialSprites;
    [SerializeField] Sprite[] ambientNoiseSprites;
    [SerializeField] Sprite[] callSprites;

    float frameTimer;

    Sprite[] pictogramSprites;

    int spriteIndex = 0;

    CalibrationPhase currentPhase;

    private void Start()
    {
        calibrator = FindObjectOfType<KinectCalibration>();

        SetCurrentSprites(calibrator.CurrentCalibrationPhase);
    }

    private void Update()
    {
        if (calibrator.CurrentCalibrationPhase != currentPhase)
        {
            SetCurrentSprites(calibrator.CurrentCalibrationPhase);
        }


        frameTimer += Time.deltaTime;

        if (frameTimer >= 1 / frameRate)
        {
            frameTimer = 0;
            spriteIndex++;
            if (spriteIndex >= pictogramSprites.Length)
            {
                spriteIndex = 0;
            }
            pictogramImage.sprite = pictogramSprites[spriteIndex];
        }
    }

    void SetCurrentSprites(CalibrationPhase calibrationPhase)
    {
        switch (calibrationPhase)
        {
            case CalibrationPhase.Stand:
                pictogramSprites = standSprites;
                break;
            case CalibrationPhase.HeadForward:
                pictogramSprites = headForwardSprites;
                break;
            case CalibrationPhase.RotLeft:
                pictogramSprites = rotLeftSprites;
                break;
            case CalibrationPhase.RotRight:
                pictogramSprites = rotRightSprites;
                break;
            case CalibrationPhase.Jump:
                pictogramSprites = jumpSprites;
                break;
            case CalibrationPhase.Glide:
                pictogramSprites = glideSprites;
                break;
            case CalibrationPhase.Squat:
                pictogramSprites = squatSprites;
                break;
            case CalibrationPhase.Hatch:
                pictogramSprites = hatchSprites;
                break;
            case CalibrationPhase.DropEgg:
                pictogramSprites = dropEggSprites;
                break;
            case CalibrationPhase.DropEggTutorial:
                pictogramSprites = dropEggTutorialSprites;
                break;
            case CalibrationPhase.AmbientNoise:
                pictogramSprites = ambientNoiseSprites;
                break;
            case CalibrationPhase.Call:
                pictogramSprites = callSprites;
                break;
        }
        currentPhase = calibrationPhase;
    }
}