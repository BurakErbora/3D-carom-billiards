using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    // More a helper class than controller, stops the ball when the rigidbody bugs and keeps a small velocity on the ball.
    [RequireComponent(typeof(Rigidbody))]
    public class BallVelocityController : MonoBehaviour
    {
        [Range(0f, 0.05f)]
        public float stopBallTreshold = 0.03f;

        private Rigidbody _ballRB;

        private void Awake()
        {
            _ballRB = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_ballRB.velocity.sqrMagnitude > 0f && _ballRB.velocity.sqrMagnitude < stopBallTreshold)
                _ballRB.velocity = Vector3.zero;
        }
    }
}