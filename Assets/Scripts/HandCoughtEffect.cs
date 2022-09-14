using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HandCoughtEffect : MonoBehaviour
{

    public MeshRenderer meshRenderer;

    public Gradient gradient;

    Sequence handCoughtSeq;


    private void OnEnable()
    {
        handCoughtSeq = DOTween.Sequence();

        handCoughtSeq.Append(meshRenderer.material.DOGradientColor(gradient, 1f));

        
    }


}
