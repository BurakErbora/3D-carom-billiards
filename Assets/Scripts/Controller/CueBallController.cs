using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    [RequireComponent(typeof(Rigidbody))]
    public class CueBallController : MonoBehaviourWithStates
    {
        [Header("Hit Force")]
        public float forceTimerTick = 0.5f;
        public float baseForce = 500f;
        public float maximumForceTicks = 10;

        [Header("Other Balls")] // for the replay
        public GameObject yellowTargetBall;
        public GameObject redTargetBall;

        private Rigidbody _cueBallRB;
        private Transform _cameraTransform;
        private float _currentHitForceScale;
        private float _lastHitForceScaleTime;
        private float _timeSinceLastTick;

        // for the replay
        private Vector3 _cachedYellowTargetBallPosition;
        private Vector3 _cachedRedTargetBallPosition;
        private Vector3 _cachedCueBallPosition;
        private Vector3 _cachedHitForce;

        private IEventManager _eventManager;
        private IGameSessionManager _gameSessionManager;

        private enum ControllerState { AwaitingInput, BallInMotion, Replay }

        private void Awake()
        {
            _cueBallRB = GetComponent<Rigidbody>();
            _cameraTransform = Camera.main.transform;
            _gameSessionManager = ServiceLocator.Current.Get<IGameSessionManager>();
            _eventManager = ServiceLocator.Current.Get<IEventManager>();
            _timeSinceLastTick = Time.timeSinceLevelLoad;


            // Setup the state machine
            stateHolder.updateDelegates = new StateHolder.UpdateDelegate[3];
            stateHolder.updateDelegates[(int)ControllerState.AwaitingInput] = UpdateAwaitingInput;
            stateHolder.updateDelegates[(int)ControllerState.BallInMotion] = UpdateBallInMotion;
            stateHolder.updateDelegates[(int)ControllerState.Replay] = UpdateReplay;

            stateHolder.SetState((int)ControllerState.AwaitingInput);
        }

        private void OnEnable()
        {
            _eventManager.StartListening(Constants.GUI_REPLAY_BUTTON_CLICKED, OnReplayButtonClicked);
        }

        private void OnDisable()
        {
            _eventManager.StopListening(Constants.GUI_REPLAY_BUTTON_CLICKED, OnReplayButtonClicked);
        }

        #region Update States
        private void UpdateAwaitingInput()
        {
            DoTimerUpdate();

            // Track space keypress. When space is held, scale the to-be-applied force with the hit force tick setting.
            if (Input.GetKey(KeyCode.Space))
            {
                if (_lastHitForceScaleTime == -1)
                    _lastHitForceScaleTime = Time.timeSinceLevelLoad;

                else if (Time.timeSinceLevelLoad - _lastHitForceScaleTime >= forceTimerTick)
                {
                    _currentHitForceScale = Mathf.Min(_currentHitForceScale + 1, maximumForceTicks);
                    _lastHitForceScaleTime = Time.timeSinceLevelLoad;
                    _eventManager.TriggerEvent(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, _currentHitForceScale / maximumForceTicks);
                }
            }
            // when released, apply the final force
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //Set cached info for replay
                _cachedCueBallPosition = transform.position;
                _cachedYellowTargetBallPosition = yellowTargetBall.transform.position;
                _cachedRedTargetBallPosition = redTargetBall.transform.position;
                _cachedHitForce = CalculateHitForce();

                _cueBallRB.AddForce(_cachedHitForce);
                _currentHitForceScale = 0f;
                _lastHitForceScaleTime = -1;
                _gameSessionManager.SetShotsTaken(_gameSessionManager.GetShotsTaken() + 1);
                
                _eventManager.TriggerEvent(Constants.SESSION_DATA_SHOTS_UPDATED);
                _eventManager.TriggerEvent(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, 0f);
                _eventManager.TriggerEvent(Constants.GUI_REPLAY_DISABLED);

                stateHolder.SetState((int)ControllerState.BallInMotion);
            }
        }

        private void UpdateBallInMotion()
        {
            DoTimerUpdate();

            if (_cueBallRB.velocity.sqrMagnitude > 0f)
            {
                //TO-DO: check score
            }
            else
            {
                _eventManager.TriggerEvent(Constants.GUI_REPLAY_ENABLED);
                stateHolder.SetState((int)ControllerState.AwaitingInput);
            }
        }

        private void UpdateReplay()
        {
            DoTimerUpdate();

            if (_cueBallRB.velocity.sqrMagnitude <= 0f)
            {
                _eventManager.TriggerEvent(Constants.GUI_REPLAY_ENABLED);
                stateHolder.SetState((int)ControllerState.AwaitingInput);
            }
        }

        #endregion Update States

        private void DoTimerUpdate()
        {
            // Update time in 1 second intervals.
            if (Time.timeSinceLevelLoad - _timeSinceLastTick >= 1f)
            {
                _gameSessionManager.SetTimePlayed(_gameSessionManager.GetTimePlayed() + 1);
                _eventManager.TriggerEvent(Constants.SESSION_DATA_TIME_UPDATED);
                _timeSinceLastTick = Time.timeSinceLevelLoad;
            }
        }

        private Vector3 CalculateHitForce()
        {
            return _cameraTransform.forward * _currentHitForceScale * baseForce;
        }

        private void OnReplayButtonClicked()
        {
            redTargetBall.transform.position = _cachedRedTargetBallPosition;
            yellowTargetBall.transform.position = _cachedYellowTargetBallPosition;
            transform.position = _cachedCueBallPosition;

            _cueBallRB.AddForce(_cachedHitForce);
            
            _eventManager.TriggerEvent(Constants.GUI_REPLAY_DISABLED);
            stateHolder.SetState((int)ControllerState.Replay);
        }
    }
}