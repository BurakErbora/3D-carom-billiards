using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    // More a helper class than controller, stops the ball when the rigidbody bugs and keeps a small velocity on the ball.
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        [Range(0f, 0.05f)]
        public float stopBallTreshold = 0.03f;

        [HideInInspector]
        public Rigidbody ballRB;

        private void Awake()
        {
            ballRB = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (ballRB.velocity.sqrMagnitude > 0f && ballRB.velocity.sqrMagnitude < stopBallTreshold)
                ballRB.velocity = Vector3.zero;
        }
    }
}