using UnityEngine;

public class ChickenCallSoundManager : MonoBehaviour
{
    Chicken chicken;

    ChickenAbilitySetup[] abilities;

    void Start()
    {
        chicken = GetComponent<Chicken>();

        abilities = GetComponents<ChickenAbilitySetup>();

        chicken.OnCall += PlayCallSound;
    }

    private void PlayCallSound()
    {
        foreach (ChickenAbilitySetup ability in abilities)
        {
            //ability.PlayCallSound();
        }
    }
}