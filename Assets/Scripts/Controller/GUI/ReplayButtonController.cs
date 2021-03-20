using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaromBilliards3D.Controller.GUI
{
    [RequireComponent(typeof(Button))]
    public class ReplayButtonController : MonoBehaviour
    {
        private Button _replayButton;
        public TextMeshProUGUI replayText;

        private IEventManager _eventManager;

        private void Awake()
        {
            _replayButton = GetComponent<Button>();
            _eventManager = ServiceLocator.Resolve<IEventManager>();

        }

        private void OnEnable()
        {
            if (replayText)
            {
                replayText.gameObject.SetActive(false);
                _eventManager.StartListening(Constants.GUI_REPLAY_STATE_CHANGED, OnReplayStateToggled);
            }
            _replayButton.onClick.AddListener(OnReplayButtonClick);
            _eventManager.StartListening(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, OnReplayButtonToggled);
            
        }

        private void OnDisable()
        {
            _replayButton.onClick.RemoveListener(OnReplayButtonClick);
            _eventManager.StopListening(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, OnReplayButtonToggled);

            if (replayText)
                _eventManager.StopListening(Constants.GUI_REPLAY_STATE_CHANGED, OnReplayStateToggled);
        }


        private void OnReplayButtonToggled(object isEnabled)
        {
            _replayButton.interactable = (bool)isEnabled;

            
        }

        private void OnReplayStateToggled(object isReplaying)
        {
            replayText.gameObject.SetActive((bool)isReplaying);
        }

        private void OnReplayButtonClick()
        {
            _eventManager.TriggerEvent(Constants.GUI_REPLAY_BUTTON_CLICKED);
        }
    }
}