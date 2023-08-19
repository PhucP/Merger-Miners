using UnityEngine;
using System.Collections;

namespace EpicToonFX
{

    public class EtfxMouseOrbit : MonoBehaviour
    {
        public Transform target;
        public float distance = 12.0f;
        public float xSpeed = 120.0f;
        public float ySpeed = 120.0f;
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;
        public float distanceMin = 8f;
        public float distanceMax = 15f;
        public float smoothTime = 2f;
        private float _rotationYAxis = 0.0f;
        private float _rotationXAxis = 0.0f;
        private float _velocityX = 0.0f;
        private float _maxVelocityX = 0.1f;
        private float _velocityY = 0.0f;
        private readonly float _autoRotationSmoothing = 0.02f;

        [HideInInspector] public bool isAutoRotating = false;
        [HideInInspector] public EtfxEffectController etfxEffectController;
        [HideInInspector] public EtfxEffectControllerPooled etfxEffectControllerPooled;

        private void Start()
        {
            Vector3 angles = transform.eulerAngles;
            _rotationYAxis = angles.y;
            _rotationXAxis = angles.x;

            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().freezeRotation = true;
            }
        }

        private void Update()
        {
            if(target)
            {
                if (Input.GetMouseButton(1))
                {
                    _velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
                    _velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;

                    if (isAutoRotating)
                    {
                        StopAutoRotation();
                    }
                }

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 15, distanceMin, distanceMax);
            }
        }

		private void FixedUpdate()
        {
            if (target)
            {
                _rotationYAxis += _velocityX;
                _rotationXAxis -= _velocityY;
                _rotationXAxis = ClampAngle(_rotationXAxis, yMinLimit, yMaxLimit);
                Quaternion toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);
                Quaternion rotation = toRotation;
                
                if (Physics.Linecast(target.position, transform.position, out RaycastHit hit))
                {
                   distance -= hit.distance;
                }

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = Vector3.Lerp(transform.position, rotation * negDistance + target.position, 0.6f);

                transform.rotation = rotation;
                transform.position = position;
                _velocityX = Mathf.Lerp(_velocityX, 0, Time.deltaTime * smoothTime);
                _velocityY = Mathf.Lerp(_velocityY, 0, Time.deltaTime * smoothTime);
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        public void InitializeAutoRotation()
        {
            isAutoRotating = true;

            StartCoroutine(AutoRotate());
        }

        public void SetAutoRotationSpeed(float rotationSpeed)
        {
            _maxVelocityX = rotationSpeed;
        }

        private void StopAutoRotation()
        {
            if (etfxEffectController != null)
                etfxEffectController.autoRotation = false;

            if (etfxEffectControllerPooled != null)
                etfxEffectControllerPooled.autoRotation = false;

            isAutoRotating = false;
            StopAllCoroutines();
        }

        IEnumerator AutoRotate()
        {
            int lerpSteps = 0;

            while (lerpSteps < 30)
            {
                _velocityX = Mathf.Lerp(_velocityX, _maxVelocityX, _autoRotationSmoothing);

                yield return new WaitForFixedUpdate();
            }

            while (isAutoRotating)
            {
                _velocityX = _maxVelocityX;

                yield return new WaitForFixedUpdate();
            }
        }
    }
}