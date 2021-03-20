using CaromBilliards3D.Controller;
using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Manager
{
    public class GameManager : MonoBehaviourWithStates
    {
        [Header("Timer Calculation")]
        public bool pauseTimerDuringReplay;
        public bool pauseTimerDuringShot;

        [Header("Hit Force")]
        public float forceTimerTick = 0.01f;
        public float baseForce = 1f;
        public float maximumForceTicks = 100;

        [Header("Balls")]
        public BallController cueBall;
        public BallController yellowTargetBall;
        public BallController redTargetBall;
        

        private Rigidbody _cueBallRB;
        private Rigidbody _yellowTargetBallRB;
        private Rigidbody _redTargetBallRB;
        private Transform _cameraTransform;
        private float _currentHitForceScale;
        private float _lastHitForceScaleTime;
        private float _timeSinceLastTick;

        private bool _isAddForceQueued; // to apply the force in FixedUpdate
        private bool _isBallMoving;
        private bool _wasBallMoving;

        // for the replay
        private Vector3 _cachedYellowTargetBallPosition;
        private Vector3 _cachedRedTargetBallPosition;
        private Vector3 _cachedCueBallPosition;
        private Vector3 _cachedHitForce;

        

        private IEventService _eventService;
        private IGameSessionService _gameSessionService;
        private IGameSettingsService _gameSettingsService;

        private enum ControllerState { AwaitingInput, BallInMotion, Replay }

        private void Awake()
        {
            _cueBallRB = cueBall.GetComponent<Rigidbody>();
            _yellowTargetBallRB = yellowTargetBall.GetComponent<Rigidbody>();
            _redTargetBallRB = redTargetBall.GetComponent<Rigidbody>();

            if (!_cueBallRB || !_yellowTargetBallRB || !_redTargetBallRB)
                Debug.LogError("All balls must have an attached Rigidbody!");

            _cameraTransform = Camera.main.transform;

            _gameSessionService = ServiceLocator.Resolve<IGameSessionService>();
            _gameSettingsService = ServiceLocator.Resolve<IGameSettingsService>();
            _gameSettingsService.LoadGameSettings(Constants.DIRECTORY_PATH_SAVES, Constants.FILE_NAME_SETTINGS, Constants.EXTENSION_SAVE_FILES);
            _eventService = ServiceLocator.Resolve<IEventService>();

            _cachedCueBallPosition = cueBall.transform.position;
            _cachedRedTargetBallPosition = redTargetBall.transform.position;
            _cachedYellowTargetBallPosition = yellowTargetBall.transform.position;

            // Setup the state machine
            stateHolder.updateDelegates = new StateHolder.UpdateDelegate[3];
            stateHolder.updateDelegates[(int)ControllerState.AwaitingInput] = UpdateAwaitingInput;
            stateHolder.updateDelegates[(int)ControllerState.BallInMotion] = UpdateBallInMotion;
            stateHolder.updateDelegates[(int)ControllerState.Replay] = UpdateReplay;
        }

        private void OnEnable()
        {
            _eventService.StartListening(Constants.GUI_REPLAY_BUTTON_CLICKED, OnReplayButtonClicked);
            _gameSessionService.ResetSession();
            stateHolder.SetState((int)ControllerState.AwaitingInput);
        }

        private void OnDisable()
        {
            _eventService.StopListening(Constants.GUI_REPLAY_BUTTON_CLICKED, OnReplayButtonClicked);
        }

        private void FixedUpdate()
        {
            if (_isAddForceQueued)
            {
                _cueBallRB.AddForce(_cachedHitForce, ForceMode.Impulse);
                _isAddForceQueued = false;
                _eventService.TriggerEvent(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, false);
                return; // skip one physics update to calculate _isBallMoving, so the applied force can be taken into account
            }

            _isBallMoving = _cueBallRB.velocity.sqrMagnitude != 0f;

            if (_wasBallMoving != _isBallMoving) // if the ball moving state just changed
            {
                if (!_isBallMoving) // if the ball has just come to a stop
                {
                    _eventService.TriggerEvent(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, true);
                    _eventService.TriggerEvent(Constants.GUI_REPLAY_STATE_CHANGED, false);
                    stateHolder.SetState((int)ControllerState.AwaitingInput);

                }

                _wasBallMoving = _isBallMoving;
            }
        }

        #region Update States
        private void UpdateAwaitingInput()
        {
            DoTimerUpdate();

            // Track space keypress. When space is held, scale the to-be-applied force with the hit force tick setting.
            if (Input.GetKey(KeyCode.Space))
            {
                _eventService.TriggerEvent(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, false);

                if (_lastHitForceScaleTime == -1)
                    _lastHitForceScaleTime = Time.timeSinceLevelLoad;

                else if (Time.timeSinceLevelLoad - _lastHitForceScaleTime >= forceTimerTick)
                {
                    _currentHitForceScale = Mathf.Min(_currentHitForceScale + 1, maximumForceTicks);
                    _lastHitForceScaleTime = Time.timeSinceLevelLoad;
                    _eventService.TriggerEvent(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, _currentHitForceScale / maximumForceTicks);
                }
            }
            // when released, apply the final force
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _eventService.TriggerEvent(Constants.GUI_REPLAY_POSSIBILITY_CHANGED, true);

                //Set cached info for replay
                _cachedCueBallPosition = cueBall.transform.position;
                _cachedYellowTargetBallPosition = yellowTargetBall.transform.position;
                _cachedRedTargetBallPosition = redTargetBall.transform.position;
                _cachedHitForce = CalculateHitForce();

                _isAddForceQueued = true;

                _currentHitForceScale = 0f;
                _lastHitForceScaleTime = -1;

                _gameSessionService.SetShotsTaken(_gameSessionService.GetShotsTaken() + 1);
                
                _eventService.TriggerEvent(Constants.SESSION_DATA_SHOTS_UPDATED);
                _eventService.TriggerEvent(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANGED, 0f);

                stateHolder.SetState((int)ControllerState.BallInMotion);
            }
        }

        private void UpdateBallInMotion()
        {
            if (!pauseTimerDuringShot)
                DoTimerUpdate();

            //Debug.Log($"_hitBallsDuringShot: {_hitBallsDuringShot.Count}");
        }

        private void UpdateReplay()
        {
            if (!pauseTimerDuringReplay)
                DoTimerUpdate();
        }

        #endregion Update States

        private void DoTimerUpdate()
        {
            // Update time in 1 second intervals.
            if (Time.timeSinceLevelLoad - _timeSinceLastTick >= 1f)
            {
                _gameSessionService.SetTimePlayed(_gameSessionService.GetTimePlayed() + 1);
                _eventService.TriggerEvent(Constants.SESSION_DATA_TIME_UPDATED);
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
            _redTargetBallRB.velocity = Vector3.zero;
            _redTargetBallRB.angularVelocity = Vector3.zero;

            yellowTargetBall.transform.position = _cachedYellowTargetBallPosition;
            _yellowTargetBallRB.velocity = Vector3.zero;
            _yellowTargetBallRB.angularVelocity = Vector3.zero;

            cueBall.transform.position = _cachedCueBallPosition;
            _cueBallRB.velocity = Vector3.zero;
            _cueBallRB.angularVelocity = Vector3.zero;

            _isAddForceQueued = true;

            _eventService.TriggerEvent(Constants.GUI_REPLAY_STATE_CHANGED, true);
            stateHolder.SetState((int)ControllerState.Replay);
        }
    }
}