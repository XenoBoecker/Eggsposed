using UnityEngine;

public class ChickenSFX : MonoBehaviour
{
    AudioSource audioSource;

    BaseChickenController bcc;
    Chicken chicken;

    bool wasSittingLastFrame;
    bool hadEggLastFrame;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bcc = GetComponent<BaseChickenController>();
        chicken = GetComponent<Chicken>();

        bcc.OnFinishBreeding += PlayHatchingSound;
    }

    private void PlayHatchingSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.eggHatchingSound, audioSource);
    }

    private void Update()
    {
        if (bcc.sitting && !wasSittingLastFrame) SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.sitDownSound, audioSource);
        else if (!bcc.sitting && wasSittingLastFrame) SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.standUpSound, audioSource);

        if (chicken.HasEgg && hadEggLastFrame) SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.pickUpEggSound, audioSource);
        else if (!chicken.HasEgg && hadEggLastFrame) SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.dropEggSound, audioSource);


        wasSittingLastFrame = bcc.sitting;
        hadEggLastFrame = chicken.HasEgg;
    }

}
