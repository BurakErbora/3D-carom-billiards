#if UNITY_EDITOR
using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class BootstrapLocaterServiceForSceneDebug : MonoBehaviour
{
    private IEventManager _eventManager;

    private void Awake()
    {
        ServiceLocator.Initiailze();

        // Register services here
        ServiceLocator.Current.Register<IGameSettingsManager>(new GameSettingsManager());
        ServiceLocator.Current.Register<IGameSessionManager>(new GameSessionManager());
        ServiceLocator.Current.Register<IEventManager>(new EventManager());

        _eventManager = ServiceLocator.Resolve<IEventManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)) //modify and use contents for quick testing
        {
            Debug.Log("Triggering Game Over");
            _eventManager.TriggerEvent(Constants.GAME_OVER);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            float randomValue = Random.Range(0f, 1f);
            Debug.Log($"Triggering BallHitBall with Random value: {randomValue}");
            _eventManager.TriggerEvent(Constants.AUDIO_BALL_HIT_BALL, randomValue);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            float randomValue = Random.Range(0f, 1f);
            Debug.Log($"Triggering BallHitWall with Random value: {randomValue}"); 
            _eventManager.TriggerEvent(Constants.AUDIO_BALL_HIT_WALL, randomValue);
        }
    }


}
#endif

