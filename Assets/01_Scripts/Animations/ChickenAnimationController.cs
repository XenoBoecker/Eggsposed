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
        cst.OnStartGlide += () => anim.SetTrigger("OnStartGlide");
        cst.OnStopGlide += () => anim.SetTrigger("OnStopGlide");
        cst.OnStartFalling += () => anim.SetTrigger("OnStartFalling");
        cst.OnStopFalling += () => anim.SetTrigger("OnStopFalling");
        cst.OnStartWalking += () => anim.SetTrigger("OnStartWalking");
        cst.OnStopWalking += () => anim.SetTrigger("OnStopWalking");
        cst.OnStandUp += () => anim.SetTrigger("OnStandUp");
        cst.OnSitDown += () => anim.SetTrigger("OnSitDown");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
