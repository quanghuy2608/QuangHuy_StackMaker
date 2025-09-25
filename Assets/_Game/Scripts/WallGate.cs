using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallGate : MonoBehaviour
{
    [SerializeField] private float wallHeight = 1f; 
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider wallCollider;

    public float WallHeight => wallHeight; 

    private bool isPassed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPassed) return;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            //So gach can = chieu cao tuong / chieu cao gach
            int neededBricks = Mathf.CeilToInt(wallHeight / player.GetBrickHeight());
            
            if (player.BrickCount >= neededBricks)
            {
                player.ClearAllBricks(); 

                DisableWall();
                isPassed = true;
            }

            else
            {
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    //Tat wallgate
    public void DisableWall()
    {
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (wallCollider != null) wallCollider.enabled = false;
    }
}

