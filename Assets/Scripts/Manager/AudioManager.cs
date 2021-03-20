using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Manager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public AudioClip ballHitBallClip;
        public AudioClip ballHitWallClip;

        private IEventManager _eventManager;
        private IGameSettingsManager _gameSettingsManager;
        private AudioSource _audiosource;

        private void Awake()
        {
            _eventManager = ServiceLocator.Resolve<IEventManager>();
            _gameSettingsManager = ServiceLocator.Resolve<IGameSettingsManager>();
            _audiosource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _eventManager.StartListening(Constants.AUDIO_BALL_HIT_BALL, OnBallHitBall);
            _eventManager.StartListening(Constants.AUDIO_BALL_HIT_WALL, OnBallHitWall);
        }

        private void OnDisable()
        {
            _eventManager.StopListening(Constants.AUDIO_BALL_HIT_BALL, OnBallHitBall);
            _eventManager.StopListening(Constants.AUDIO_BALL_HIT_WALL, OnBallHitWall);
        }

        private void OnBallHitBall(object interpolatedForce) // interpolatedForce: float force as between 0 - 1 based on game logic (not including volume)
        {
            _audiosource.clip = ballHitBallClip;
            _audiosource.volume = (float)interpolatedForce * _gameSettingsManager.gameSettings.audioVolume;
            //Debug.Log($"Ball hit sound. interpolatedForce: {interpolatedForce}, audioVolume: {_gameSettingsManager.gameSettings.audioVolume} - Result: {_audiosource.volume}");
            _audiosource.Play();
        }

        private void OnBallHitWall(object interpolatedForce) // interpolatedForce: float force as between 0 - 1 based on game logic (not including volume)
        {
            _audiosource.clip = ballHitWallClip;
            _audiosource.volume = (float)interpolatedForce * _gameSettingsManager.gameSettings.audioVolume;
            //Debug.Log($"Ball hit sound. interpolatedForce: {interpolatedForce}, audioVolume: {_gameSettingsManager.gameSettings.audioVolume} - Result: {_audiosource.volume}");
            _audiosource.Play();
        }
    }
}