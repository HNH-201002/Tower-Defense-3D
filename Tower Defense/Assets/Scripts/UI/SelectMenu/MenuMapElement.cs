using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMapElement : MonoBehaviour
{
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private Button button;
    [SerializeField] private GameObject[] star;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text nameIndexMapText;
    private const string SFX_CLICK = "Click";
    private void Awake()
    {
        lockPanel.SetActive(true);
    }
    public void SetData(int stars,int health,int totalHealth,int index)
    {
        for (int i = 0; i < stars; i++)
        {
            star[i].SetActive(true);
        }
        healthText.text = $"{health}/{totalHealth}";
        nameIndexMapText.text = index.ToString();
    }
    public void SetLockPanel()
    {
        lockPanel.SetActive(false);
    }
    public void SetScene(int index)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Scene index out of range: " + index);
            return;
        }
        if (button != null)
        {
            button.onClick.AddListener(()=> StartCoroutine(LoadSceneAsync(index)));
        }
    }
    IEnumerator LoadSceneAsync(int index)
    {
        SoundManager.Instance.PlaySound(SFX_CLICK);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
