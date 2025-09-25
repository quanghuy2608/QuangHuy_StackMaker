using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    //Play Button
    public void PlayGame()
    {
        string levelName = LevelManager.Instance.GetCurrentLevel();
        //Debug.Log(levelName);

        if (string.IsNullOrEmpty(levelName))
        {
            //Debug.LogError("khong load duoc level");
            return;
        }

        SceneManager.LoadScene(levelName);
    }

}
