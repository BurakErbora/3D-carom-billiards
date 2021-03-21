using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaromBilliards3D.Controller.GUI
{
    [RequireComponent(typeof(Button))]
    public class ReplayButtonController : MonoBehaviour
    {
        public TextMeshProUGUI replayText;

        private Button _replayButton;
        private IEventService _eventService;

        private void Awake()
        {
            _replayButton = GetComponent<Button>();
            _eventService = ServiceLocator.Resolve<IEventService>();

        }

        private void OnEnable()
        {
            if (replayText)
            {
                replayText.gameObject.SetActive(false);
                _eventService.StartListening(Constants.GUI_REPLAY_STATE_CHANGED, OnReplayStateToggled);
            }
            _replayButton.onClick.AddListener(OnReplayButtonClick);
            _eventService.StartListening(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, OnReplayButtonToggled);
            
        }

        private void OnDisable()
        {
            _replayButton.onClick.RemoveListener(OnReplayButtonClick);
            _eventService.StopListening(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, OnReplayButtonToggled);

            if (replayText)
                _eventService.StopListening(Constants.GUI_REPLAY_STATE_CHANGED, OnReplayStateToggled);
        }


        private void OnReplayButtonToggled(object isEnabled)
        {
            _replayButton.interactable = (bool)isEnabled;
        }

        private void OnReplayStateToggled(object isReplaying) // there might be cases where the Replay button is disabled but the game is not in replay mode (like ball force buildup). 
                                                              // thus OnReplayButtonToggled and OnReplayStateToggled are seperate callbacks.
        {
            replayText.gameObject.SetActive((bool)isReplaying);
        }

        private void OnReplayButtonClick()
        {
            _eventService.TriggerEvent(Constants.GUI_REPLAY_BUTTON_CLICKED);
        }
    }
}