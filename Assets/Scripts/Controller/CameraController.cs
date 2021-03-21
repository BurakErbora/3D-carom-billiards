using UnityEngine;

namespace CaromBilliards3D.Controller
{
    public class CameraController : MonoBehaviour
    {
        public GameObject cameraFocus;
        public float rotationSpeed = 5.0f;
        public float clampMin;
        public float clampMax;

        private Vector3 _rotationEuler;
        private float _cameraRotationOffset;

        private void Awake()
        {
            if (cameraFocus == null)
            {
                Debug.LogError("CameraFocus object is not assigned");
            }

            transform.position = cameraFocus.transform.position;
            _rotationEuler = transform.rotation.eulerAngles;
            _cameraRotationOffset = Camera.main.transform.localRotation.eulerAngles.x;
        }


        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                _rotationEuler.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
                _rotationEuler.x = Mathf.Clamp(_rotationEuler.x, -_cameraRotationOffset, 90-_cameraRotationOffset);
                _rotationEuler.y += Input.GetAxis("Mouse X") * rotationSpeed;
                transform.eulerAngles = _rotationEuler;
            }

            transform.position = cameraFocus.transform.position;
        }
    }
}

