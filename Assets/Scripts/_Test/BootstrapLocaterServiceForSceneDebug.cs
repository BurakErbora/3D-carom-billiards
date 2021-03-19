using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class BootstrapLocaterServiceForSceneDebug : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        ServiceLocator.Initiailze();

        // Register services here
        ServiceLocator.Current.Register<IGameManager>(new GameManager());
        ServiceLocator.Current.Register<IEventManager>(new EventManager());
    }
#endif
}

