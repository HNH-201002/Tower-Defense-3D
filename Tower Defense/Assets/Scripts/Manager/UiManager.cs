using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject uiInGame;
    bool toggle;
    private const string SFX_CLICK = "Click";
    private void Start()
    {
        uiInGame.SetActive(false);
    }
    //event
    public void PauseButton()
    {
        SoundManager.Instance.PlaySound(SFX_CLICK);
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
    public void ContinueButton()
    {
        SoundManager.Instance.PlaySound(SFX_CLICK);
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index > SceneManager.sceneCount)
        {
            StartCoroutine(LoadAsyncScene(0));
        }
        else
        {
            StartCoroutine(LoadAsyncScene(index));
        }
    }
    public void QuitButton()
    {
        SoundManager.Instance.PlaySound(SFX_CLICK);
        StartCoroutine(LoadAsyncScene(0));
    }
    public void RestartButton()
    {
        SoundManager.Instance.PlaySound(SFX_CLICK);
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadAsyncScene(index));
    }

    IEnumerator LoadAsyncScene(int index)
    {
        Time.timeScale = 1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
