using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnimationController : MonoBehaviour
{
    Animator anim;
    ChickenStateTracker cst;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cst = GetComponent<ChickenStateTracker>();

        cst.OnJump += () => anim.SetTrigger("OnJump");
        cst.OnStartGlide += () => anim.SetBool("Gliding", true);
        cst.OnStopGlide += () => anim.SetBool("Gliding", false);
        cst.OnStartFalling += () => anim.SetBool("Falling", true);
        cst.OnStopFalling += () => anim.SetBool("Falling", false);
        cst.OnStartWalking += () => anim.SetBool("Walking", true);
        cst.OnStopWalking += () => anim.SetBool("Walking", false);
        cst.OnSitDown += () => anim.SetBool("Sitting", true);
        cst.OnStandUp += () => anim.SetBool("Sitting", false);

        cst.OnStartGlide += () => anim.SetTrigger("OnStartGliding");
        cst.OnStartFalling += () => anim.SetTrigger("OnStartFalling");
        cst.OnStartWalking += () => anim.SetTrigger("OnStartWalking");
        cst.OnSitDown += () => anim.SetTrigger("OnSitDown");
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("IdleIndex", Random.Range(0, 5));
        anim.SetInteger("BreedIndex", Random.Range(0, 2));

        anim.SetFloat("LeaningDirection", cst.LeaningDirection);

        anim.SetBool("WalkClosedWings", Random.Range(0, 2) == 0);
    }
}
