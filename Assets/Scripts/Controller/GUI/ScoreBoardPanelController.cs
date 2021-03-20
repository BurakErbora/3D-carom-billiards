using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CaromBilliards3D.Controller.GUI
{
    public class ScoreBoardPanelController : MonoBehaviour
    {
        public TextMeshProUGUI totalTimeText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI shotsTakenText;

        private IEventManager _eventManager;
        private IGameSessionManager _gameSessionManager;

        private void Awake()
        {
            _eventManager = ServiceLocator.Resolve<IEventManager>();
            _gameSessionManager = ServiceLocator.Resolve<IGameSessionManager>();
        }

        private void OnEnable()
        {
            _eventManager.StartListening(Constants.SESSION_DATA_TIME_UPDATED, UpdateTimeText);
            _eventManager.StartListening(Constants.SESSION_DATA_SHOTS_UPDATED, UpdateShotsText);
            _eventManager.StartListening(Constants.SESSION_DATA_SCORE_UPDATED, UpdateScoreText);
        }

        private void OnDisable()
        {
            _eventManager.StopListening(Constants.SESSION_DATA_TIME_UPDATED, UpdateTimeText);
            _eventManager.StopListening(Constants.SESSION_DATA_SHOTS_UPDATED, UpdateShotsText);
            _eventManager.StopListening(Constants.SESSION_DATA_SCORE_UPDATED, UpdateScoreText);
        }

        private void UpdateTimeText()
        {
            totalTimeText.text = $"{_gameSessionManager.GetTimePlayed()} s";
        }

        private void UpdateShotsText()
        {
            shotsTakenText.text = $"{_gameSessionManager.GetShotsTaken()}";
        }        

        private void UpdateScoreText()
        {
            scoreText.text = $"{_gameSessionManager.GetScore()}";
        }

    }
}