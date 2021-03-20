using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaromBilliards3D.Controller.GUI
{
    [RequireComponent(typeof(Button))]
    public class ReplayButtonController : MonoBehaviour
    {
        private Button _replayButton;
        private IEventManager _eventManager; 

        private void Awake()
        {
            _replayButton = GetComponent<Button>();
            _eventManager = ServiceLocator.Resolve<IEventManager>();
            
        }

        private void OnEnable()
        {
            _replayButton.onClick.AddListener(OnReplayButtonClick);
            _eventManager.StartListening(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, OnReplayToggled);
        }

        private void OnDisable()
        {
            _replayButton.onClick.RemoveListener(OnReplayButtonClick);
            _eventManager.StopListening(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, OnReplayToggled);
        }


        private void OnReplayToggled(object isEnabled)
        {
            _replayButton.interactable = (bool)isEnabled;
        }

        private void OnReplayButtonClick()
        {
            _eventManager.TriggerEvent(Constants.GUI_REPLAY_BUTTON_CLICKED);
        }
    }
}