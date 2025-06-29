using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Player
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField, MinValue(0f)] private float _movementSpeed;
        [SerializeField, MinValue(0f)] private float _rotationSpeed;
        [SerializeField] private InputActionReference _moveAxis;

        public bool IsActive { get; set; }
        public bool IsMoving => _motion.sqrMagnitude > 0.01f;
        private Vector3 _motion;

        private void Update()
        {
            var input = _moveAxis.action.ReadValue<Vector2>().normalized;
            _motion.x = input.x;
            _motion.z = input.y;

            _characterController.Move(_movementSpeed * Time.deltaTime * _motion);

            if (IsMoving)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(_motion),
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
