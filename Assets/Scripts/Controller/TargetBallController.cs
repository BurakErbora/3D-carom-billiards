using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
     public class TargetBallController : BallController
    {
        private BallController _currentlyHitBall;

        private void OnCollisionEnter(Collision collision)
        {
            _currentlyHitBall = collision.gameObject.GetComponent<BallController>();

            if (_currentlyHitBall)
                // The interpolation here with 100f as max sqrMagnitude is purely empirical.
                // In most sensible scenarios sqrMagnitude will be between 10 and 500, and in some extreme speeds will be a lot more (around 2000).
                // I clamp these cases as 500 max as well here. Note that this interpolates for the impact velocity only. The game auido setting is globally
                // applied in the AuidioManager.
                eventService.TriggerEvent(Constants.AUDIO_BALL_HIT_BALL, Mathf.Clamp01(ballRB.velocity.sqrMagnitude / 500f));

            else // if not a ball, we must be hitting a wall
                eventService.TriggerEvent(Constants.AUDIO_BALL_HIT_WALL, Mathf.Clamp01(ballRB.velocity.sqrMagnitude / 500f));
        }
    }
}