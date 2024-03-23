using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private List<AudioClip> clips;
    private Dictionary<string, AudioClip> clipsDictionary = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioMixerGroup audioMixer;
    [SerializeField] private AudioSource audioSFX;
    [SerializeField] private AudioSource audioSound;
    private Dictionary<string, string> sceneToBackgroundSound = new Dictionary<string, string>
    {
        {"M_1", "M_BG"},
        {"M_2", "M_BG"},
        {"M_3", "M_BG"},
        {"MapSelect","Menu_BG"}
    };

    private Queue<AudioSource> availableSources = new Queue<AudioSource>();
    private AudioSource[] allOfSourceSFX;
    [SerializeField] private int poolSize = 10;

    private bool isBackgroundSoundMuted = false;
    private bool isSFXMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var clip in clips)
            {
                clipsDictionary.Add(clip.name, clip);
            }
            audioSound.loop = true;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        AudioSource source;
        allOfSourceSFX = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = audioMixer;
            source.playOnAwake = false;
            source.loop = false;
            allOfSourceSFX[i] = source;
            availableSources.Enqueue(source);
        }
    }
    public void PlaySound(string clipName, bool loop = false)
    {
        if (clipsDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource source = GetPooledAudioSource();
            if (source != null)
            {
                source.clip = clip;
                source.loop = loop;
                source.Play();

                // Nếu âm thanh không loop, hãy trả AudioSource vào pool sau khi nó kết thúc
                if (!loop)
                {
                    StartCoroutine(ReturnSourceToPoolAfterClipEnds(source, clip.length));
                }
            }
            else
            {
                Debug.LogWarning("No available AudioSource in the pool to play: " + clipName);
            }
        }
        else
        {
            Debug.LogWarning("Clip not found: " + clipName);
        }
    }
    private IEnumerator ReturnSourceToPoolAfterClipEnds(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        if (source != null)
        {
            source.Stop();
            source.clip = null;
            availableSources.Enqueue(source);
        }
    }

    private AudioSource GetPooledAudioSource()
    {
        if (availableSources.Count > 0)
        {
            return availableSources.Dequeue();
        }
        return null;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneToBackgroundSound.TryGetValue(scene.name, out string backgroundSoundName))
        {
            PlayBackgroundMusic(backgroundSoundName);
        }
        else
        {
            Debug.LogWarning("No background sound mapping found for scene: " + scene.name);
        }
    }

    public void PlayBackgroundMusic(string backgroundSoundName)
    {
        if (clipsDictionary.TryGetValue(backgroundSoundName, out AudioClip clip))
        {
            if (audioSound.clip != clip)
            {
                audioSound.clip = clip;
                audioSound.Play();
            }
        }
        else
        {
            Debug.LogWarning("Background music clip not found: " + backgroundSoundName);
        }
    }
    public void SetBackgroundSoundState(bool state)
    {
        isBackgroundSoundMuted = !state; 
        ToggleBackgroundSound(); 
    }

    public void SetSfxSoundState(bool state)
    {
        isSFXMuted = !state; 
        ToggleSFX(); 
    }

    public void ToggleBackgroundSound()
    {
        isBackgroundSoundMuted = !isBackgroundSoundMuted;
        audioSound.mute = isBackgroundSoundMuted;
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;
        foreach (AudioSource source in allOfSourceSFX)
        {
            source.mute = isSFXMuted;
        }
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
}