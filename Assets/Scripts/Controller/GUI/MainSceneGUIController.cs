using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Controller.GUI
{
    public class MainSceneGUIController : MonoBehaviour
    {
        public GameObject gamePlayGUI;
        public GameObject gameOverGUI;

        private IEventService _eventService;

        private void Awake()
        {
            _eventService = ServiceLocator.Resolve<IEventService>();
        }

        private void OnEnable()
        {
            _eventService.StartListening(Constants.GAME_OVER, OnGameOver);
        }

        private void OnDisable()
        {
            _eventService.StopListening(Constants.GAME_OVER, OnGameOver);
        }

        private void OnGameOver()
        {
            gamePlayGUI.SetActive(false);
            gameOverGUI.SetActive(true);
        }
    }
}