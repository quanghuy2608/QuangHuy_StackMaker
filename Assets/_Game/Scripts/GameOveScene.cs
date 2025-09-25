using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    //PlayAgain Button
    public void PlayAgain()
    {
        if (LevelManager.Instance != null)
        {
            string currentLevel = LevelManager.Instance.GetCurrentLevel();
            //Debug.Log(currentLevel);
            SceneManager.LoadScene(currentLevel); 
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
    //Menu Button
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
