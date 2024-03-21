using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    [SerializeField] private int _health;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] GameObject[] starSprites; // win star
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private GameObject gameOverPanel;
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
        UpdateHealthText();
        gameOverPanel.SetActive(false);
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
    }
    private void UpdateHealthText()
    {
        _healthText.text = _health.ToString();
    }
    public void Win()
    {
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
        titleText.text = "VICTORY";
        gameOverPanel.SetActive(true);
    }
    private IEnumerator DelayedDefeated(float delay)
    {
        yield return new WaitForSeconds(delay);
        Defeated();
    }
    public void Defeated()
    {
        titleText.text = "Defeated";
        gameOverPanel.SetActive(true);
    }
}
