using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CaromBilliards3D.Utility;
using CaromBilliards3D.Services;
using TMPro;

namespace CaromBilliards3D.Controller.GUI
{
    public class StartMenuController : MonoBehaviour
    {
        public Slider volumeSlider;
        public TextMeshProUGUI totalShotsTakenText;
        public TextMeshProUGUI totalTimeText;

        private IGameSettingsManager _gameSettingsManager;
        private IGameSessionManager _gameSessionManager;

        private void Awake()
        {
            _gameSettingsManager = ServiceLocator.Resolve<IGameSettingsManager>();
            _gameSessionManager = ServiceLocator.Resolve<IGameSessionManager>();

            // Load the game settings from the previously saved session if it exists (for now audio volume only).

            _gameSettingsManager.LoadGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);
            _gameSessionManager.LoadGameSessionData(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_LAST_SESSION, Constants.EXTENSION_SAVE_FILES);
            
            UpdateUIFromGameSettings();
            UpdateUIFromGameSessionData();

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
        private void UpdateUIFromGameSessionData()
        {
            totalShotsTakenText.text = $"{_gameSessionManager.GetShotsTaken()}";
            totalTimeText.text = $"{_gameSessionManager.GetTimePlayed()} s";
        }

        private void SaveSettings()
        {
            // Save settings to a JSON file to persist between game sessions (and be read from during main game).
            _gameSettingsManager.SaveGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);
        }
    }
}