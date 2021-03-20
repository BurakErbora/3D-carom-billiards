using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Controller.GUI
{
    public class MainSceneGUIController : MonoBehaviour
    {
        public GameObject gamePlayGUI;
        public GameObject gameOverGUI;

        private IEventManager _eventmanager;

        private void Awake()
        {
            _eventmanager = ServiceLocator.Resolve<IEventManager>();
        }

        private void OnEnable()
        {
            _eventmanager.StartListening(Constants.GAME_OVER, OnGameOver);
        }

        private void OnDisable()
        {
            _eventmanager.StopListening(Constants.GAME_OVER, OnGameOver);
        }

        private void OnGameOver()
        {
            gamePlayGUI.SetActive(false);
            gameOverGUI.SetActive(true);
        }
    }
}