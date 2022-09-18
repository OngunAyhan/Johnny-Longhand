using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
public class RopeTensionFeedback : MonoBehaviour
{
    public float minTension = 0;
    public float maxTension = 0.2f;
    public Color normalColor = Color.white;
    public Color tensionColor = Color.red;


    private ObiRope rope;
    private Material localMaterial;

    float ropeLength;
    float treshold;
    
    private void Start()
    {
        rope = GetComponent<ObiRope>();
        localMaterial = GetComponent<MeshRenderer>().material;
        float maxLength = FindObjectOfType<ArmScript>().maxLength;
        treshold = maxLength * (70f / 100f);
    }
    
    void Update()
    {

        ropeLength = rope.restLength;

        if (ropeLength > treshold)
        {

            float tension = rope.CalculateLength() / rope.restLength - 1;

            float lerpFactor = (tension - minTension) / (maxTension - minTension);
            localMaterial.color = Color.Lerp(normalColor, tensionColor, lerpFactor * 5f);
        }
        else
        {
            localMaterial.color = normalColor;
        }
    }
}
