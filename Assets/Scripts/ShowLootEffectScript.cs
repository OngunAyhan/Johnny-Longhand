using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowLootEffectScript : MonoBehaviour
{
    
    void Start()
    {
        transform.DOScale(1.1f, 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}
