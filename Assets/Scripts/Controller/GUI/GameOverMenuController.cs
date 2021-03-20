using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;

namespace CaromBilliards3D.Controller.GUI
{
    public class GameOverMenuController : MonoBehaviour
    {
        public TextMeshProUGUI totalShotsTakenText;
        public TextMeshProUGUI totalTimeText;
        private IGameSessionService _gameSessionService;

        private void Awake()
        {
            _gameSessionService = ServiceLocator.Resolve<IGameSessionService>();
        }

        private void OnEnable() // When this gui is enabled, it's game over for sure end nothing else. Do all the game over game logic here.
        {
            totalShotsTakenText.text = _gameSessionService.GetShotsTaken().ToString();
            totalTimeText.text = $"{_gameSessionService.GetTimePlayed()} s";
            _gameSessionService.SaveGameSessionData(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_LAST_SESSION);
        }

        public void OnReturnToMainMenuClicked()
        {
            SceneManager.LoadScene(Constants.SCENE_BUILD_INDEX_START_MENU);
        }

        public void OnPlayAgainClicked()
        {
            SceneManager.LoadScene(Constants.SCENE_BUILD_INDEX_MAIN);
        }


    }
}