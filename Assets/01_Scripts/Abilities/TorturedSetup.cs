using UnityEngine;

public class TorturedSetup : ChickenAbilitySetup
{

    [SerializeField] GameObject bloodEffect;

    ParticleSystem bloodEffectParticles;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        bloodEffectParticles = Instantiate(bloodEffect, transform).GetComponentInChildren<ParticleSystem>();
    }

    protected override void Update()
    {
        base.Update();

        if(movement.isGrounded && !bloodEffectParticles.isPlaying)
        {
            bloodEffectParticles.Play();
        }
        else if (!movement.isGrounded && bloodEffectParticles.isPlaying)
        {
            bloodEffectParticles.Stop();
        }


        if (CanCall())
        {
            print("Torture: canCall");
            chicken.Call(false);
        }
    }
}