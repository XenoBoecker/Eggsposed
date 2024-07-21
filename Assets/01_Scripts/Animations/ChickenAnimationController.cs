using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnimationController : MonoBehaviour
{
    Animator anim;
    ChickenStateTracker cst;


    [SerializeField] float callAnimTime = 1f;
    float timeSinceLastCall;

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
        cst.OnCall += OnCall;

        cst.OnStartGlide += () => anim.SetTrigger("OnStartGliding");
        cst.OnStartFalling += () => anim.SetTrigger("OnStartFalling");
        cst.OnStartWalking += () => anim.SetTrigger("OnStartWalking");
        cst.OnSitDown += () => anim.SetTrigger("OnSitDown");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastCall < callAnimTime)
        {
            timeSinceLastCall += Time.deltaTime;
            anim.SetBool("Calling", true);
        }else{
            anim.SetBool("Calling", false);
        }

        anim.SetInteger("IdleIndex", Random.Range(0, 5));
        anim.SetInteger("BreedIndex", Random.Range(0, 2));

        anim.SetFloat("LeaningDirection", cst.LeaningDirection);

        anim.SetBool("WalkClosedWings", Random.Range(0, 2) == 0);
    }

    void OnCall(bool v)
    {
        anim.SetTrigger("Call");
        timeSinceLastCall = 0;
    }
}
