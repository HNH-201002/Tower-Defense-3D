using UnityEngine;

public class bl_LoadingScreenExample : MonoBehaviour
{
    public string SceneName = "LoadExample";
    public bool loadOnStart = false;

    private bool loaded = false;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if (loadOnStart)
        {
            bl_SceneLoaderManager.LoadScene(SceneName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (!loaded && Input.GetKeyDown(KeyCode.Space))
        {
            bl_SceneLoaderManager.LoadScene(SceneName);
            loaded = true;
        }
    }
}