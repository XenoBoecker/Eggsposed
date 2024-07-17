using System;
using System.Collections;
using UnityEngine;

public class ChickenCallVFX : MonoBehaviour
{
    Chicken chicken;



    [SerializeField] SpriteRenderer callSpriteRenderer;
    [SerializeField] Sprite[] callSprites;


    [SerializeField] float callSpriteDuration = 0.3f;


    [SerializeField] int totalSpriteRotations = 2;

    private void Start()
    {
        chicken = GetComponent<Chicken>();
        chicken.OnCall += PlayCallVFX;

        callSpriteRenderer.enabled = false;
    }

    private void PlayCallVFX()
    {
        StartCoroutine(AnimateCallSpriteRenderer());
    }

    IEnumerator AnimateCallSpriteRenderer()
    {
        callSpriteRenderer.enabled = true;

        for (int i = 0; i < totalSpriteRotations; i++)
        {
            for (int j = 0; j < callSprites.Length; j++)
            {
                callSpriteRenderer.sprite = callSprites[j];
                yield return new WaitForSeconds(callSpriteDuration);
            }
        }

        callSpriteRenderer.enabled = false;
    }
}