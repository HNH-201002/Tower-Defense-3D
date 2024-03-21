using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject uiInGame;
    bool toggle;
    private void Start()
    {
        uiInGame.SetActive(false);
    }
    //event
    public void PauseButton()
    {
        toggle = !toggle;
        uiInGame.SetActive(toggle);
        if (uiInGame.gameObject.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void RestartButton()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        Time.timeScale = 1;
        string sceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
