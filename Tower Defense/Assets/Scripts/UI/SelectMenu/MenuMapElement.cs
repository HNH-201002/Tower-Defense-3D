using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMapElement : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject[] star;
    [SerializeField] private TMP_Text healthText;
    public TMP_Text nameIndexMapText;
    private const string SFX_CLICK = "Click";
    private void Awake()
    {
        button.interactable = false;
    }
    public void SetData(int stars,int health,int totalHealth,int index,bool isUnlock)
    {
        for (int i = 0; i < stars; i++)
        {
            star[i].SetActive(true);
        }
        healthText.text = $"{health}/{totalHealth}";
        nameIndexMapText.text = index.ToString();
        if (isUnlock)
        {
            SetLockPanel();
        }
    }
    public void SetLockPanel()
    {
        button.interactable = true;
    }
    public void SetScene(string name)
    {
        if (button != null)
        {
            button.onClick.AddListener(()=> StartCoroutine(LoadSceneAsync(name)));
        }
    }
    IEnumerator LoadSceneAsync(string name)
    {
        SoundManager.Instance.PlaySound(SFX_CLICK);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
