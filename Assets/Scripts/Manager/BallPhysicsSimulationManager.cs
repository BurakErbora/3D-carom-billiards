using CaromBilliards3D.Controller;
using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaromBilliards3D.Manager
{
    [RequireComponent(typeof(LineRenderer))]
    public class BallPhysicsSimulationManager : MonoBehaviour
    {
        [Header("General")]
        public int maxSimulationIterations = 30;

        [Header("Balls")]
        public BallController cueBall;
        public BallController yellowTargetBall;
        public BallController redTargetBall;

        private Rigidbody _cueBallRB;
        private Rigidbody _yellowTargetBallRB;
        private Rigidbody _redTargetBallRB;

        //private GameObject _simulatedCueBall;
        //private GameObject _simulatedCueBall;
        //private GameObject _simulatedCueBall;

        private Rigidbody _simulatedCueBallRB;
        private Rigidbody _simulatedYelllowTargetBallRB;
        private Rigidbody _simulatedRedTargetBallRB;

        private PhysicsScene _currentPhysicsScene;
        private Scene _simulationScene;
        private PhysicsScene _simulationPhysicsScene;
        private LineRenderer _lineRenderer;
        private Vector3 _cachedForce;

        private IEventService _eventService;

        private void Awake()
        {
            if (cueBall == null || yellowTargetBall == null || redTargetBall == null)
                Debug.LogError("All balls not assigned!");

            _cueBallRB = cueBall.GetComponent<Rigidbody>();
            _yellowTargetBallRB = yellowTargetBall.GetComponent<Rigidbody>();
            _redTargetBallRB = redTargetBall.GetComponent<Rigidbody>();

            _currentPhysicsScene = SceneManager.GetActiveScene().GetPhysicsScene();
            _lineRenderer = GetComponent<LineRenderer>();

            _eventService = ServiceLocator.Resolve<IEventService>();
        }

        private void OnEnable()
        {
            CreateSimulationScene();
            CreateSimulationSceneObjects();

            _eventService.StartListening(Constants.CUE_BALL_HIT_FORCE_VALUE_CHANGED, SimulateBallTrajectory);
            _eventService.StartListening(Constants.SESSION_DATA_SHOTS_UPDATED, CleanSimulatedBallTrajectory);
        }

        private void OnDisable()
        {
            if (_simulationScene != null)
                SceneManager.UnloadSceneAsync(_simulationScene); // Destroys scene GOs as well.

            _eventService.StopListening(Constants.CUE_BALL_HIT_FORCE_VALUE_CHANGED, SimulateBallTrajectory);
            _eventService.StopListening(Constants.SESSION_DATA_SHOTS_UPDATED, CleanSimulatedBallTrajectory);
        }

        void FixedUpdate()
        {
            if (!Physics.autoSimulation && _currentPhysicsScene.IsValid())
                _currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }

        public void CreateSimulationScene()
        {
            _simulationScene = SceneManager.CreateScene(Constants.SCENE_NAME_RUNTIME_PHYSICS_SIMULATION, new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            _simulationPhysicsScene = _simulationScene.GetPhysicsScene();
        }

        public void CreateSimulationSceneObjects()
        {
            if (_simulationScene == null)
            {
                Debug.LogWarning("Simulation scene was null while creating simulation scene objects. Creating simulation scene first...");
                CreateSimulationScene();
            }

            Collider[] sceneColliders = FindObjectsOfType<Collider>();

            List<Transform> scenePhysicsObjects = new List<Transform>();

            // add unique objects from the found colliders
            foreach (Collider collider in sceneColliders)
            {
                if (!scenePhysicsObjects.Contains(collider.transform))
                    scenePhysicsObjects.Add(collider.transform);
            }

            foreach (Transform physicsObject in scenePhysicsObjects) //MoveGameObjectToScene works for only GOs that are at scene root!
            {
                GameObject simulationObject = Instantiate(physicsObject.gameObject);
                simulationObject.transform.position = physicsObject.position;
                simulationObject.transform.rotation = physicsObject.rotation;

                // strip renderers
                Renderer simulationObjectRenderer = simulationObject.GetComponent<Renderer>();
                if (simulationObjectRenderer)
                    Destroy(simulationObjectRenderer);

                // strip the ball conrollers
                BallController ballController = simulationObject.GetComponent<BallController>();
                if (ballController != null)
                    Destroy(ballController);

                // cache simulated ball references to the cue ball;
                if (physicsObject.gameObject == cueBall.gameObject) 
                    _simulatedCueBallRB = simulationObject.GetComponent<Rigidbody>();
                else if (physicsObject.gameObject == yellowTargetBall.gameObject)
                    _simulatedYelllowTargetBallRB = simulationObject.GetComponent<Rigidbody>();
                else if (physicsObject.gameObject == redTargetBall.gameObject)
                    _simulatedRedTargetBallRB = simulationObject.GetComponent<Rigidbody>();

                SceneManager.MoveGameObjectToScene(simulationObject, _simulationScene);
                Debug.Log($"Added Simulation object: {simulationObject.name}");

            }
        }

        public void SimulateBallTrajectory(object force)
        {
            Physics.autoSimulation = false;

            // skip simulation if result will be the same
            if (_cachedForce == (Vector3)force)
                return;
            else
                _cachedForce = (Vector3)force;

            if (_currentPhysicsScene.IsValid() && _simulationPhysicsScene.IsValid())
            {
                ResetBallTrajectory();
                _simulatedCueBallRB.AddForce((Vector3)force, ForceMode.Impulse);
                _lineRenderer.positionCount = maxSimulationIterations + 1;
                _lineRenderer.SetPosition(0, cueBall.transform.position);

                for (int i = 0; i < maxSimulationIterations; i++)
                {
                    _simulationScene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);
                    _lineRenderer.SetPosition(i + 1, _simulatedCueBallRB.transform.position);
                }
            }

            Physics.autoSimulation = true;
        }

        private void ResetBallTrajectory()
        {
            _simulatedCueBallRB.transform.position = cueBall.transform.position;
            _simulatedCueBallRB.transform.rotation = cueBall.transform.rotation;
            _simulatedCueBallRB.velocity = _cueBallRB.velocity;

            _simulatedYelllowTargetBallRB.transform.position = yellowTargetBall.transform.position;
            _simulatedYelllowTargetBallRB.transform.rotation = yellowTargetBall.transform.rotation;
            _simulatedYelllowTargetBallRB.velocity = _yellowTargetBallRB.velocity;

            _simulatedRedTargetBallRB.transform.position = redTargetBall.transform.position;
            _simulatedRedTargetBallRB.transform.rotation = redTargetBall.transform.rotation;
            _simulatedRedTargetBallRB.velocity = _redTargetBallRB.velocity;
        }

        private void CleanSimulatedBallTrajectory()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}