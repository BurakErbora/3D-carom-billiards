using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CaromBilliards3D.Utility;
using CaromBilliards3D.DataModel;

namespace CaromBilliards3D.UI
{
    public class StartMenuControl : MonoBehaviour
    {
        public Slider volumeSlider;

        private GameSettings _gameSettings; // TO-DO: Move to a persistent singleton manager to use throughout game session.

        private void Awake()
        {
            // Load the game settings from previously saved session if it exists (for now audio volume only).

            if (SaveLoadUtility.LoadJsonData(out _gameSettings, Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES))
                UpdateUIFromGameSettings();
            else
                _gameSettings = new GameSettings();
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
            _gameSettings.audioVolume = volumeSlider.value;
        }
        private void UpdateUIFromGameSettings()
        {
            volumeSlider.value = _gameSettings.audioVolume;
        }

        private void SaveSettings()
        {
            // Save settings to JSON file to persist between game sessions (and be read from during main game).
            SaveLoadUtility.SaveJsonData(_gameSettings, Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);
        }
    }
}