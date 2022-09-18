using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HandCoughtEffect : MonoBehaviour
{

    public SkinnedMeshRenderer skinnedMeshRenderer;

    public Gradient gradient;

    Sequence handCoughtSeq;

    public GameObject HandNormal;

    public GameObject HandBlendShaped;

    private void OnEnable()
    {
        HandNormal.SetActive(false);

        HandBlendShaped.SetActive(true);

        handCoughtSeq = DOTween.Sequence();

        handCoughtSeq.Append(skinnedMeshRenderer.material.DOGradientColor(gradient, 1f));

        float blendAmount = 0;

        DOTween.To(() => blendAmount, x => blendAmount = x, 100, 1f).OnUpdate(() => { skinnedMeshRenderer.SetBlendShapeWeight(0, blendAmount); }).SetEase(Ease.OutElastic);
        
    }


}
