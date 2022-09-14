using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperObsPanel : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public Material WarningMat;

    public Material TransparentMat;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshRenderer.material = WarningMat;
        }
        else
        {
            meshRenderer.material = TransparentMat;
        }
    }
}
