using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CaromBilliards3D.Utility;
using CaromBilliards3D.Services;

namespace CaromBilliards3D.Controller.GUI
{
    public class StartMenuController : MonoBehaviour
    {
        public Slider volumeSlider;

        private IGameSettingsManager _gameSettingsManager;

        private void Awake()
        {
            // Cache the game manager service for convenience
            _gameSettingsManager = ServiceLocator.Current.Get<IGameSettingsManager>();
            
            // Load the game settings from the previously saved session if it exists (for now audio volume only).
            _gameSettingsManager.LoadGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);

            UpdateUIFromGameSettings();

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
            _gameSettingsManager.gameSettings.audioVolume = volumeSlider.value;
        }
        private void UpdateUIFromGameSettings()
        {
            volumeSlider.value = _gameSettingsManager.gameSettings.audioVolume;
        }

        private void SaveSettings()
        {
            // Save settings to a JSON file to persist between game sessions (and be read from during main game).
            _gameSettingsManager.SaveGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);
        }
    }
}