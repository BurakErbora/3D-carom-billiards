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
            _eventManager = ServiceLocator.Current.Get<IEventManager>();
            _gameSessionManager = ServiceLocator.Current.Get<IGameSessionManager>();
        }

        private void OnEnable()
        {
            _eventManager.StartListening(Constants.SESSION_DATA_TIME_UPDATED, UpdateTimeText);
            _eventManager.StartListening(Constants.SESSION_DATA_SHOTS_UPDATED, UpdateShotsText);
        }

        private void OnDisable()
        {
            _eventManager.StopListening(Constants.SESSION_DATA_TIME_UPDATED, UpdateTimeText);
            _eventManager.StopListening(Constants.SESSION_DATA_SHOTS_UPDATED, UpdateShotsText);
        }

        private void UpdateTimeText()
        {
            totalTimeText.text = $"{_gameSessionManager.GetTimePlayed()} s";
        }

        private void UpdateShotsText()
        {
            shotsTakenText.text = $"{_gameSessionManager.GetShotsTaken()}";
        }

    }
}