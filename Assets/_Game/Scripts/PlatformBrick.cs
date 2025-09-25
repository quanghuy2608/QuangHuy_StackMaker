using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBrick : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider boxCollider;

    public void DisableBrick()
    {
        //Xu li disable brick renderer va collider
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
