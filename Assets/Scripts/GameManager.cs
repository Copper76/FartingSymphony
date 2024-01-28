using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Scene nextScene;
    private AsyncOperation asyncLoad;
    private AsyncOperation asyncUnload;
    private float lastRestartTick;
    // Start is called before the first frame update
    public void LoadLevel(string nextSceneName)
    {

        StartCoroutine(NextLevel(nextSceneName));
        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator NextLevel(string nextSceneName)
    {
        nextScene = SceneManager.GetSceneByName(nextSceneName);

        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + asyncLoad.progress);
            yield return null;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void toggle(GameObject menu)
    {
        menu.SetActive(!menu.activeInHierarchy);
    }
}
