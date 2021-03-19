using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CaromBilliards3D.Utility;
//using CaromBilliards3D.DataModel;
using CaromBilliards3D.Services;
using System;

namespace CaromBilliards3D.UI
{
    public class StartMenuControl : MonoBehaviour
    {
        public Slider volumeSlider;

        private IGameManager _gameManager;

        private void Awake()
        {
            // Cache the game manager service for convenience
            _gameManager = ServiceLocator.Current.Get<IGameManager>();
            
            // Load the game settings from the previously saved session if it exists (for now audio volume only).
            _gameManager.LoadGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);

            if (_gameManager.gameSettings != null)
                UpdateUIFromGameSettings();
            else
                _gameManager.InitializeGameSettings();
        }

        private void OnEnable()
        {
            volumeSlider.onValueChanged.AddListener((f) => UpdateGameSettingsFromUI());
        }

        private void OnDisable()
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
        }

        public void OnStartNewGame()
        {
            SaveSettings();
            SceneManager.LoadScene(Constants.SCENE_BUILD_INDEX_MAIN);
        }

        private void UpdateGameSettingsFromUI() 
        {
            _gameManager.gameSettings.audioVolume = volumeSlider.value;
        }
        private void UpdateUIFromGameSettings()
        {
            volumeSlider.value = _gameManager.gameSettings.audioVolume;
        }

        private void SaveSettings()
        {
            // Save settings to a JSON file to persist between game sessions (and be read from during main game).
            _gameManager.SaveGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);
        }
    }
}