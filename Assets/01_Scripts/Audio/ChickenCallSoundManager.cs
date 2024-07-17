using UnityEngine;

public class ChickenCallSoundManager : MonoBehaviour
{
    AudioSource audioSource;
    Chicken chicken;


    [SerializeField] ChickenData hydraChicken;

    [SerializeField] ChickenData torturedChicken;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        chicken = GetComponent<Chicken>();

        chicken.OnCall += PlayCallSound;
    }

    private void PlayCallSound()
    {
        if (chicken.BodyData == hydraChicken)
        {
            SoundManager.Instance.PlaySound(chicken.BodyData.callSound, audioSource);
        }
        else if (chicken.BodyData == torturedChicken)
        {
            SoundManager.Instance.PlaySound(chicken.BodyData.callSound, audioSource);
        }
        else
        {
            SoundManager.Instance.PlaySound(chicken.HeadData.callSound, audioSource);
        }

    }
}