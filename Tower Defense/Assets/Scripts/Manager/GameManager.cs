using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    [SerializeField] private int health;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] GameObject[] starSprites; // win star
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject defeatPanel;
    private int _health;

    private const string SFX_VICTORY = "Victory";
    private const string SFX_LOOSE_HEALTH = "LooseLife";
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        _health = health;
        UpdateHealthText();
        winPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }
    public void DecreaseHealth()
    {
        _health--;
        if (_health <= 0)
        {
            _health = 0;
            UpdateHealthText();
            StartCoroutine(DelayedDefeated(1));
        }
        else
        {
            UpdateHealthText();
        }
        SoundManager.Instance.PlaySound(SFX_LOOSE_HEALTH);
    }
    private void UpdateHealthText()
    {
        _healthText.text = _health.ToString();
    }
    public void Win()
    {
        SoundManager.Instance.PlaySound(SFX_VICTORY);
        int starsAwarded;

        if (_health >= 18)
        {
            starsAwarded = 3;
        }
        else if (_health > 10)
        {
            starsAwarded = 2;
        }
        else
        {
            starsAwarded = 1;
        }
        for (int i = 0; i < starsAwarded; i++)
        {
            starSprites[i].SetActive(true);
        }
        winPanel.SetActive(true);
        MapData data = new MapData(SceneManager.GetActiveScene().name, starsAwarded,_health,health,true);
        SaveManager.SaveData(data);
        Time.timeScale = 0;
    }

    private IEnumerator DelayedDefeated(float delay)
    {
        yield return new WaitForSeconds(delay);
        Defeated();
    }
    public void Defeated()
    {
        defeatPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
