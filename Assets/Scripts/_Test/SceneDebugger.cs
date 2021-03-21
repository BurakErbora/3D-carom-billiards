#if UNITY_EDITOR
using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Test
{
    [DefaultExecutionOrder(-10000)]
    public class SceneDebugger : MonoBehaviour
    {
        private IEventService _eventService;

        private void Awake()
        {
            ServiceLocator.Initiailze();

            // Register services here
            ServiceLocator.Current.Register<IGameSettingsService>(new GameSettingsService());
            ServiceLocator.Current.Register<IGameSessionService>(new GameSessionService());
            ServiceLocator.Current.Register<IEventService>(new EventService());

            _eventService = ServiceLocator.Resolve<IEventService>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) //modify and use contents for quick testing
            {
                Debug.Log("Triggering Game Over");
                _eventService.TriggerEvent(Constants.GAME_OVER);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                float randomValue = Random.Range(0f, 1f);
                Debug.Log($"Triggering BallHitBall with Random value: {randomValue}");
                _eventService.TriggerEvent(Constants.AUDIO_BALL_HIT_BALL, randomValue);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                float randomValue = Random.Range(0f, 1f);
                Debug.Log($"Triggering BallHitWall with Random value: {randomValue}");
                _eventService.TriggerEvent(Constants.AUDIO_BALL_HIT_WALL, randomValue);
            }
        }
    }
}
#endif

