using CaromBilliards3D.Services;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    // Base class for cue and target ball classes. Basically stops the ball when the rigidbody bugs and keeps a small velocity on the ball.
    // Subclasses also deal with ball collision logic.
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BallController : MonoBehaviour
    {
        [Range(0f, 0.05f)]
        public float stopBallTreshold = 0.03f;

        protected Rigidbody ballRB;

        protected IEventService eventService;

        protected virtual void Awake()
        {
            ballRB = GetComponent<Rigidbody>();
            eventService = ServiceLocator.Resolve<IEventService>();
        }

        protected virtual void Update()
        {
            if (ballRB.velocity.sqrMagnitude > 0f && ballRB.velocity.sqrMagnitude < stopBallTreshold)
                ballRB.velocity = Vector3.zero;
        }
    }
}