using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsButton : MonoBehaviour
{
    [SerializeField] private Button backgroundSoundButton;
    [SerializeField] private GameObject[] toggleAvatarBackgroundSoundButton;
    [SerializeField] private Button sfxSoundButton;
    [SerializeField] private GameObject[] toggleSfxSoundButton;

    private const string BackgroundSoundPrefKey = "BackgroundSoundEnabled";
    private const string SfxSoundPrefKey = "SfxSoundEnabled";

    private bool isBackgroundSoundOn = true;
    private bool isSFXSoundOn = true;

    private void Start()
    {
        isBackgroundSoundOn = PlayerPrefs.GetInt(BackgroundSoundPrefKey, 1) == 1;
        isSFXSoundOn = PlayerPrefs.GetInt(SfxSoundPrefKey, 1) == 1;

        SoundManager.Instance.SetBackgroundSoundState(isBackgroundSoundOn);
        SoundManager.Instance.SetSfxSoundState(isSFXSoundOn);

        backgroundSoundButton.onClick.AddListener(ToggleBackgroundSound);
        sfxSoundButton.onClick.AddListener(ToggleSfxSound);

        UpdateButtonVisuals();
        UpdateButtonVisuals();
    }

    public void ToggleBackgroundSound()
    {
        isBackgroundSoundOn = !isBackgroundSoundOn;
        SoundManager.Instance.ToggleBackgroundSound();
        PlayerPrefs.SetInt(BackgroundSoundPrefKey, isBackgroundSoundOn ? 1 : 0);
        PlayerPrefs.Save(); // Save the state immediately
        UpdateButtonVisuals();
    }

    public void ToggleSfxSound()
    {
        isSFXSoundOn = !isSFXSoundOn;
        SoundManager.Instance.ToggleSFX();
        PlayerPrefs.SetInt(SfxSoundPrefKey, isSFXSoundOn ? 1 : 0);
        PlayerPrefs.Save(); // Save the state immediately
        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        toggleAvatarBackgroundSoundButton[0].SetActive(!isBackgroundSoundOn);
        toggleAvatarBackgroundSoundButton[1].SetActive(isBackgroundSoundOn);

        toggleSfxSoundButton[0].SetActive(!isSFXSoundOn);
        toggleSfxSoundButton[1].SetActive(isSFXSoundOn);
    }
    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); 


        isBackgroundSoundOn = true;
        isSFXSoundOn = true;
        SoundManager.Instance.SetBackgroundSoundState(isBackgroundSoundOn);
        SoundManager.Instance.SetSfxSoundState(isSFXSoundOn);

        UpdateButtonVisuals();
    }
}