using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        //Load trong vong 2 giay
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MenuScene");
    }
}
