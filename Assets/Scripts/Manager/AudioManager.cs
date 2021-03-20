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

        private IEventService _eventService;
        private IGameSettingsService _gameSettingsService;
        private AudioSource _audiosource;

        private void Awake()
        {
            _eventService = ServiceLocator.Resolve<IEventService>();
            _gameSettingsService = ServiceLocator.Resolve<IGameSettingsService>();
            _audiosource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _eventService.StartListening(Constants.AUDIO_BALL_HIT_BALL, OnBallHitBall);
            _eventService.StartListening(Constants.AUDIO_BALL_HIT_WALL, OnBallHitWall);
        }

        private void OnDisable()
        {
            _eventService.StopListening(Constants.AUDIO_BALL_HIT_BALL, OnBallHitBall);
            _eventService.StopListening(Constants.AUDIO_BALL_HIT_WALL, OnBallHitWall);
        }

        private void OnBallHitBall(object interpolatedForce) // interpolatedForce: float force as between 0 - 1 based on game logic (not including volume)
        {
            _audiosource.clip = ballHitBallClip;
            _audiosource.volume = (float)interpolatedForce * _gameSettingsService.gameSettings.audioVolume;
            //Debug.Log($"Ball hit sound. interpolatedForce: {interpolatedForce}, audioVolume: {_gameSettingsService.gameSettings.audioVolume} - Result: {_audiosource.volume}");
            _audiosource.Play();
        }

        private void OnBallHitWall(object interpolatedForce) // interpolatedForce: float force as between 0 - 1 based on game logic (not including volume)
        {
            _audiosource.clip = ballHitWallClip;
            _audiosource.volume = (float)interpolatedForce * _gameSettingsService.gameSettings.audioVolume;
            //Debug.Log($"Ball hit sound. interpolatedForce: {interpolatedForce}, audioVolume: {_gameSettingsService.gameSettings.audioVolume} - Result: {_audiosource.volume}");
            _audiosource.Play();
        }
    }
}