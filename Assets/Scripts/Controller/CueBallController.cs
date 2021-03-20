using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    public class CueBallController : BallController
    {
        private List<BallController> _hitBallsDuringShot = new List<BallController>();
        private BallController _currentlyHitBall;

        private IGameSessionService _gameSessionService;

        protected override void Awake()
        {
            base.Awake();
            _gameSessionService = ServiceLocator.Resolve<IGameSessionService>();
        }

        private void OnEnable()
        {
            eventService.StartListening(Constants.SESSION_DATA_SHOTS_UPDATED, OnStartNewShot);
        }

        private void OnDisable()
        {
            eventService.StopListening(Constants.SESSION_DATA_SHOTS_UPDATED, OnStartNewShot);
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
                eventService.TriggerEvent(Constants.AUDIO_BALL_HIT_BALL, Mathf.Clamp01(ballRB.velocity.sqrMagnitude / 500f));
                //Debug.Log($"Triggering BallHitBall with sqrMagnitude: {_cueBallRB.velocity.sqrMagnitude}, resulting in: {_cueBallRB.velocity.sqrMagnitude / 500f}");

                if (!_hitBallsDuringShot.Contains(_currentlyHitBall))
                {
                    //Debug.Log("UNIQUE HIT!");
                    _hitBallsDuringShot.Add(_currentlyHitBall);
                    
                    if (_hitBallsDuringShot.Count == Constants.GAMEPLAY_TOTAL_TARGET_BALL_COUNT)
                    {
                        _gameSessionService.SetScore(_gameSessionService.GetScore() + 1);
                        eventService.TriggerEvent(Constants.SESSION_DATA_SCORE_UPDATED);
                        //Debug.Log("SCORE!");
                        if (_gameSessionService.GetScore() == Constants.GAMEPLAY_TOTAL_SCORES_TO_WIN)
                        {
                            eventService.TriggerEvent(Constants.GAME_OVER);
                            //Debug.Log("GAME OVER!");
                        }
                    }
                }
            }
            else // if not a ball, we must be hitting a wall
            {
                eventService.TriggerEvent(Constants.AUDIO_BALL_HIT_WALL, Mathf.Clamp01(ballRB.velocity.sqrMagnitude / 500f));
                //Debug.Log($"Triggering BallHitWall with sqrMagnitude: {_cueBallRB.velocity.sqrMagnitude}, resulting in: {_cueBallRB.velocity.sqrMagnitude / 500f}");
            }
        }
    }
}