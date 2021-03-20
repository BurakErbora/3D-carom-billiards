using CaromBilliards3D.Services;
using CaromBilliards3D.UI;
using CaromBilliards3D.Utility;
using UnityEngine;
using TMPro;

namespace CaromBilliards3D.Controller.GUI
{
    [RequireComponent(typeof(UIFillBar))]
    public class ForceBarController : MonoBehaviour
    {
        public TextMeshProUGUI forcePercentText;

        private UIFillBar _forceBarFill;
        private IEventService _eventService;
        private void Awake()
        {
            _forceBarFill = GetComponent<UIFillBar>();
            _eventService = ServiceLocator.Resolve<IEventService>();
        }

        private void OnEnable()
        {
            _eventService.StartListening(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, OnCueBallHitForcePercentChanged);
        }

        private void OnDisable()
        {
            _eventService.StopListening(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, OnCueBallHitForcePercentChanged);
        }

        private void OnCueBallHitForcePercentChanged(object percent)
        {
            _forceBarFill.SetFillAmount((float)percent);
            forcePercentText.text = $"{(float)percent * 100} %";
        }
    }
}
