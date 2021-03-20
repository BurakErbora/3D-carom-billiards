using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    [RequireComponent(typeof(Rigidbody))]
    public class CueBallController : MonoBehaviour
    {
        private List<BallController> _hitBallsDuringShot = new List<BallController>();
        private BallController _currentlyHitBall;
        private Rigidbody _cueBallRB;

        private IEventManager _eventManager;
        private IGameSessionManager _gameSessionManager;

        private void Awake()
        {
            _eventManager = ServiceLocator.Resolve<IEventManager>();
            _gameSessionManager = ServiceLocator.Resolve<IGameSessionManager>();

            _cueBallRB = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _eventManager.StartListening(Constants.SESSION_DATA_SHOTS_UPDATED, OnStartNewShot);
        }

        private void OnDisable()
        {
            _eventManager.StopListening(Constants.SESSION_DATA_SHOTS_UPDATED, OnStartNewShot);
        }

        private void OnStartNewShot()
        {
            _hitBallsDuringShot.Clear();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _currentlyHitBall = collision.gameObject.GetComponent<BallController>();

            if (_currentlyHitBall)
            {
                // The interpolation here with 100f as max sqrMagnitude is purely empirical.
                // In most sensible scenarios sqrMagnitude will be between 10 and 500, and in some extreme speeds will be a lot more (around 2000).
                // I clamp these cases as 500 max as well here.
                _eventManager.TriggerEvent(Constants.AUDIO_BALL_HIT_BALL, Mathf.Clamp01(_cueBallRB.velocity.sqrMagnitude / 500f));
                //Debug.Log($"Triggering BallHitBall with sqrMagnitude: {_cueBallRB.velocity.sqrMagnitude}, resulting in: {_cueBallRB.velocity.sqrMagnitude / 500f}");

                if (!_hitBallsDuringShot.Contains(_currentlyHitBall))
                {
                    //Debug.Log("UNIQUE HIT!");
                    _hitBallsDuringShot.Add(_currentlyHitBall);
                    
                    if (_hitBallsDuringShot.Count == Constants.GAMEPLAY_TOTAL_TARGET_BALL_COUNT)
                    {
                        _gameSessionManager.SetScore(_gameSessionManager.GetScore() + 1);
                        _eventManager.TriggerEvent(Constants.SESSION_DATA_SCORE_UPDATED);
                        //Debug.Log("SCORE!");
                        if (_gameSessionManager.GetScore() == Constants.GAMEPLAY_TOTAL_SCORES_TO_WIN)
                        {
                            _eventManager.TriggerEvent(Constants.GAME_OVER);
                            //Debug.Log("GAME OVER!");
                        }
                    }
                }
            }
            else // if not a ball, we must be hitting a wall
            {
                _eventManager.TriggerEvent(Constants.AUDIO_BALL_HIT_WALL, Mathf.Clamp01(_cueBallRB.velocity.sqrMagnitude / 500f));
                //Debug.Log($"Triggering BallHitWall with sqrMagnitude: {_cueBallRB.velocity.sqrMagnitude}, resulting in: {_cueBallRB.velocity.sqrMagnitude / 500f}");
            }
        }
    }
}