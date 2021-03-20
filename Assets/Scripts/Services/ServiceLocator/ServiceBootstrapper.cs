using CaromBilliards3D.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaromBilliards3D.Services
{
    public static class ServiceBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initiailze()
        {

            if (SceneManager.GetActiveScene().buildIndex != Constants.SCENE_BUILD_INDEX_INITIALIZATION)
                return;

            ServiceLocator.Initiailze();

            // Register services here
            ServiceLocator.Current.Register<IGameSettingsService>(new GameSettingsService());
            ServiceLocator.Current.Register<IGameSessionService>(new GameSessionService());
            ServiceLocator.Current.Register<IEventService>(new EventService());

            SceneManager.LoadSceneAsync(Constants.SCENE_BUILD_INDEX_START_MENU, LoadSceneMode.Additive);
        }
    }
}