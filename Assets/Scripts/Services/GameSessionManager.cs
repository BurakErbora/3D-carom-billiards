using CaromBilliards3D.DataModel;
using CaromBilliards3D.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CaromBilliards3D.Services
{
    public class GameSessionManager : IGameSessionManager
    {
        private GameSessionData _gameSessionData = new GameSessionData();

        GameSessionData IGameSessionManager.gameSessionData { get => _gameSessionData; set => _gameSessionData = value; }

        public int GetScore()
        {
            return _gameSessionData.score;
        }

        public int GetShotsTaken()
        {
            return _gameSessionData.shotsTaken;
        }

        public int GetTimePlayed() //in seconds
        {
            return _gameSessionData.timePlayed;
        }

        public void SetScore(int score)
        {
            _gameSessionData.score = score;
        }

        public void SetShotsTaken(int shotsTaken)
        {
            _gameSessionData.shotsTaken = shotsTaken;
        }

        public void SetTimePlayed(int timePlayed) // in seconds
        {
            _gameSessionData.timePlayed = timePlayed;
        }

        public void LoadGameSessionData(string directoryPath, string fileName, string fileExtension = ".dat")
        {
            SaveLoadUtility.LoadJsonData(out _gameSessionData, directoryPath, fileName, fileExtension = ".dat");

            if (_gameSessionData == null)
            {
                Debug.LogError("Game session data could not be loaded, initializing default session data");
                _gameSessionData = new GameSessionData();
            }
        }

        public void SaveGameSessionData(string directoryPath, string fileName, string fileExtension = ".dat")
        {
            SaveLoadUtility.SaveJsonData(_gameSessionData, directoryPath, fileName, fileExtension);
        }
    }
}

