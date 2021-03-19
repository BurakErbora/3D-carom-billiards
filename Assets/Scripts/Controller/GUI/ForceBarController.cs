using CaromBilliards3D.Services;
using CaromBilliards3D.UI;
using CaromBilliards3D.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CaromBilliards3D.Controller.GUI
{
    [RequireComponent(typeof(UIFillBar))]
    public class ForceBarController : MonoBehaviour
    {
        public TextMeshProUGUI forcePercentText;

        private UIFillBar _forceBarFill;
        private IEventManager _eventManager;
        private void Awake()
        {
            _forceBarFill = GetComponent<UIFillBar>();
            _eventManager = ServiceLocator.Current.Get<IEventManager>();
        }

        private void OnEnable()
        {
            _eventManager.StartListening(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, OnCueBallHitForcePercentChanged);
        }

        private void OnDisable()
        {
            _eventManager.StopListening(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, OnCueBallHitForcePercentChanged);
        }

        private void OnCueBallHitForcePercentChanged(object percent)
        {
            _forceBarFill.SetFillAmount((float)percent);
            forcePercentText.text = $"{(float)percent * 100} %";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
