using UnityEngine;
using UnityEngine.SceneManagement;

using CaromBilliards3D.Services;

namespace CaromBilliards3D.Utility
{
    public static class ServiceBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initiailze()
        {
            ServiceLocator.Initiailze();

            // Register services here
            ServiceLocator.Current.Register<IGameManager>(new GameManager());

            SceneManager.LoadSceneAsync(Constants.SCENE_BUILD_INDEX_START_MENU, LoadSceneMode.Additive);
        }
    }
}