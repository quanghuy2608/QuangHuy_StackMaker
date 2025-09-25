using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    //danh sach level
    private string[] levels = { "Level1", "Level2", "Level3", "Level4", "Level5" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //luu level hien tai
    public void SaveCurrentLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        if (System.Array.Exists(levels, l => l == currentLevel))
        {
            PlayerPrefs.SetString("CurrentLevel", currentLevel);
            PlayerPrefs.Save();
            //Debug.Log("level hien tai:" +currentLevel);
        }
       
    }

    //lay level hien tai
    public string GetCurrentLevel()
    {
        return PlayerPrefs.GetString("CurrentLevel", "Level1");
    }

    //Load level tiep theo
    public void LoadNextLevel()
    {
        string current = GetCurrentLevel();
        int idx = System.Array.IndexOf(levels, current);

        if (idx >= 0 && idx < levels.Length - 1)
        {
            string next = levels[idx + 1];
            //Debug.Log("level tiep theo:" + next);
            SceneManager.LoadScene(next);
            PlayerPrefs.SetString("CurrentLevel", next);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Het game roi, tobecontinue +))");
            SceneManager.LoadScene("MenuScene");
        }
    }
}
