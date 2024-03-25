using UnityEngine;

namespace Lovatto.SceneLoader
{
    public class bl_SceneLoaderCanvas : MonoBehaviour
    {
        private static bl_SceneLoaderCanvas ActiveSingleton = null;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            if (bl_SceneLoaderManager.IsGlobalLoadingScreen())
            {
                var sl = FindObjectsOfType<bl_SceneLoaderCanvas>();
                if(ActiveSingleton == null)
                {
                    ActiveSingleton = this;
                    DontDestroyOnLoad(gameObject);
                }

                foreach (var loader in sl)
                {
                    loader.SetActive(loader == ActiveSingleton);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            if (ActiveSingleton == this) ActiveSingleton = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}