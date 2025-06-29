using UnityEngine;

namespace Game.Entity.Vehicles
{
    public sealed class DriveByInput : MonoBehaviour
    {
        public bool IsMoving => _rigidbody.linearVelocity.magnitude > 0.1f;
        private const float DRAG_FACTOR = 0.98f;
        private const float DRIFT_FACTOR = 0.95f;

        [SerializeField] private float _acceleration = 3000f;
        [SerializeField] private float _maxSpeed = 50f;
        [SerializeField] private Rigidbody _rigidbody;

        private float _inputForward;
        private float _inputTurn;

        public void Update()
        {
            _inputForward = Input.GetAxis("Vertical");
            _inputTurn = Input.GetAxis("Horizontal");
        }

        public void FixedUpdate()
        {
            Accelerate();
            Steer();
            ApplyPhysics();
        }

        private void Accelerate()
        {
            if (_rigidbody.linearVelocity.magnitude >= _maxSpeed) return;
            _rigidbody.AddForce(_acceleration * _inputForward * Time.fixedDeltaTime * transform.forward);
        }

        private void ApplyPhysics()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.linearVelocity);
            localVelocity.x *= DRIFT_FACTOR;
            _rigidbody.linearVelocity = transform.TransformDirection(localVelocity);

            _rigidbody.linearVelocity *= DRAG_FACTOR;
        }

        private void Steer()
        {
            if (!IsMoving) return;

            float speedFactor = Mathf.Clamp01(_rigidbody.linearVelocity.magnitude / _maxSpeed);
            float movingDirection = Vector3.Dot(_rigidbody.linearVelocity, transform.forward) >= 0 ? 1f : -1f;
            float steerAmount = _inputTurn * 10f * speedFactor * movingDirection;
            Quaternion deltaRotation = Quaternion.Euler(0f, steerAmount, 0f);

            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
    }
}
