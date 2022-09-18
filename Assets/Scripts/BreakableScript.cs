using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    //[HideInInspector]
    public bool IsHitting;

    private bool IsBroken;

    public float HitCounterTime;

    private float HitCounter;

    public GameObject BrokenObject;
    
    
    void Update()
    {
        if (!IsBroken)
        {
            if (IsHitting)
            {
                HitCounter += Time.deltaTime;

                if (HitCounter >= HitCounterTime)
                {
                    IsBroken = true;
                    //BrokenObject.SetActive(true);
                    
                    FindObjectOfType<ArmScript>().FailedToRetrieveLoot(2f);
                    IsHitting = false;
                    HitCounter = 0f;
                }
            }
        }
        
    }


    
}
