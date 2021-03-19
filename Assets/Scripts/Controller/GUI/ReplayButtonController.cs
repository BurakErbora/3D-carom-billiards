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
            _eventManager = ServiceLocator.Current.Get<IEventManager>();
            
        }

        private void OnEnable()
        {
            _replayButton.onClick.AddListener(OnReplayButtonClick);
            _eventManager.StartListening(Constants.GUI_REPLAY_ENABLED, OnReplayEnabled);
            _eventManager.StartListening(Constants.GUI_REPLAY_DISABLED, OnReplayDisabled);
        }

        private void OnDisable()
        {
            _replayButton.onClick.RemoveListener(OnReplayButtonClick);
            _eventManager.StopListening(Constants.GUI_REPLAY_DISABLED, OnReplayDisabled);
            _eventManager.StopListening(Constants.GUI_REPLAY_ENABLED, OnReplayEnabled);
        }


        private void OnReplayButtonClick()
        {
            _eventManager.TriggerEvent(Constants.GUI_REPLAY_BUTTON_CLICKED);
        }

        private void OnReplayDisabled()
        {
            _replayButton.interactable = false;
            Debug.Log("Replay Button set interactible FALSE");
        }

        private void OnReplayEnabled()
        {
            _replayButton.interactable = true;
            Debug.Log("Replay Button set interactible TRUE");
        }

    }
}