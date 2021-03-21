using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using TMPro;
using UnityEngine;

namespace CaromBilliards3D.Controller.GUI
{
    public class ScoreBoardPanelController : MonoBehaviour
    {
        public TextMeshProUGUI totalTimeText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI shotsTakenText;

        private IEventService _eventService;
        private IGameSessionService _gameSessionService;

        private void Awake()
        {
            _eventService = ServiceLocator.Resolve<IEventService>();
            _gameSessionService = ServiceLocator.Resolve<IGameSessionService>();
        }

        private void OnEnable()
        {
            _eventService.StartListening(Constants.SESSION_DATA_TIME_UPDATED, UpdateTimeText);
            _eventService.StartListening(Constants.SESSION_DATA_SHOTS_UPDATED, UpdateShotsText);
            _eventService.StartListening(Constants.SESSION_DATA_SCORE_UPDATED, UpdateScoreText);
        }

        private void OnDisable()
        {
            _eventService.StopListening(Constants.SESSION_DATA_TIME_UPDATED, UpdateTimeText);
            _eventService.StopListening(Constants.SESSION_DATA_SHOTS_UPDATED, UpdateShotsText);
            _eventService.StopListening(Constants.SESSION_DATA_SCORE_UPDATED, UpdateScoreText);
        }

        private void UpdateTimeText()
        {
            totalTimeText.text = $"{_gameSessionService.GetTimePlayed()} s";
        }

        private void UpdateShotsText()
        {
            shotsTakenText.text = $"{_gameSessionService.GetShotsTaken()}";
        }        

        private void UpdateScoreText()
        {
            scoreText.text = $"{_gameSessionService.GetScore()}";
        }

    }
}