using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBrick : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider col;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();

        
        meshRenderer.enabled = false;
        col.isTrigger = true;
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }
}
