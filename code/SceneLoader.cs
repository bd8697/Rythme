using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int currSceneIdx;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadNextScene()
    {
        currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIdx + 1);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadPlaySceneAsync()
    {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        StartCoroutine(AsyncSceneLoader());
    }

    private IEnumerator AsyncSceneLoader()
    {
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >=0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLastScene()
    {
         SceneManager.LoadScene(FindObjectOfType<GameState>().LastSceneIdx);
    }
   
}
