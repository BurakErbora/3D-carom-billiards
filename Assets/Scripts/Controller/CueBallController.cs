using CaromBilliards3D.Services;
using CaromBilliards3D.Utility;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CueBallController : MonoBehaviour
{
    [Header("Hit Force")]
    public float forceTimerTick = 0.5f;
    public float baseForce = 500f;
    public float maximumForceTicks = 10;

    private Rigidbody _ballRB;
    private Transform _cameraTransform;
    private float _currentHitForceScale;
    private float _lastHitForceScaleTime;

    private IEventManager _eventManager;
    
    private void Awake()
    {
        _ballRB = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        _eventManager = ServiceLocator.Current.Get<IEventManager>();
    }

    void Update()
    {
        // Track space keypress. When space is held, scale the to-be-applied force with the hit force tick setting.
        if (Input.GetKey(KeyCode.Space))
        {
            if (_lastHitForceScaleTime == -1)
                _lastHitForceScaleTime = Time.timeSinceLevelLoad;

            else if (Time.timeSinceLevelLoad - _lastHitForceScaleTime >= forceTimerTick)
            {
                _currentHitForceScale = Mathf.Min(_currentHitForceScale + 1, maximumForceTicks);
                _lastHitForceScaleTime = Time.timeSinceLevelLoad;
                _eventManager.TriggerEvent(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANED, _currentHitForceScale / maximumForceTicks);
            }
                
        }
        // when released, apply the final force
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _ballRB.AddForce(CalculateHitForce());
            _currentHitForceScale = 0f;
            _lastHitForceScaleTime = -1;
            _eventManager.TriggerEvent(Constants.CUE_BALL_HIT_FORCE_PERCENT_CHANED, 0f);
        }
    }

    private Vector3 CalculateHitForce()
    {
        return _cameraTransform.forward * _currentHitForceScale * baseForce;
    }
}
