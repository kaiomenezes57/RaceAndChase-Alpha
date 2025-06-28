using Game.Events;
using Game.Interaction;
using Game.Utils.Collision;
using log4net.Util;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Components
{
    #region ABSTRACT
    public interface IEntityComponent 
    {
        bool IsActive { get; set; }
    }

    public interface IEntityComponentInitializer : IEntityComponent
    {
        void Initialize(MonoBehaviour owner);
    }

    public interface IEntityComponentUpdater : IEntityComponent
    {
        void Update(MonoBehaviour owner);
    }

    public interface IEntityComponentFixedUpdater : IEntityComponent
    {
        void FixedUpdate(MonoBehaviour owner);
    }

    public interface IEntityComponentCleaner : IEntityComponent
    {
        void Clear(MonoBehaviour owner);
    }
    #endregion

    public sealed class DriveByInput_EntityComponent : IEntityComponentUpdater, IEntityComponentFixedUpdater
    {
        public bool IsActive { get; set; }
        public bool IsMoving => _rigidbody.linearVelocity.magnitude > 0.1f;
        private const float DRAG_FACTOR = 0.98f;
        private const float DRIFT_FACTOR = 0.95f;

        [SerializeField] private float _acceleration = 3000f;
        [SerializeField] private float _maxSpeed = 50f;
        [SerializeField] private Rigidbody _rigidbody;

        private float _inputForward;
        private float _inputTurn;

        public void Update(MonoBehaviour owner)
        {
            _inputForward = Input.GetAxis("Vertical");
            _inputTurn = Input.GetAxis("Horizontal");
        }

        public void FixedUpdate(MonoBehaviour owner)
        {
            Accelerate(owner);
            Steer(owner);
            ApplyPhysics(owner);
        }

        private void Accelerate(MonoBehaviour owner)
        {
            if (_rigidbody.linearVelocity.magnitude >= _maxSpeed) return;
            _rigidbody.AddForce(_acceleration * _inputForward * Time.fixedDeltaTime * owner.transform.forward);
        }

        private void ApplyPhysics(MonoBehaviour owner)
        {
            Vector3 localVelocity = owner.transform.InverseTransformDirection(_rigidbody.linearVelocity);
            localVelocity.x *= DRIFT_FACTOR;
            _rigidbody.linearVelocity = owner.transform.TransformDirection(localVelocity);

            _rigidbody.linearVelocity *= DRAG_FACTOR;
        }

        public void Steer(MonoBehaviour owner)
        {
            if (!IsMoving) return;

            float speedFactor = Mathf.Clamp01(_rigidbody.linearVelocity.magnitude / _maxSpeed);
            float movingDirection = Vector3.Dot(_rigidbody.linearVelocity, owner.transform.forward) >= 0 ? 1f : -1f;
            float steerAmount = _inputTurn * 10f * speedFactor * movingDirection;
            Quaternion deltaRotation = Quaternion.Euler(0f, steerAmount, 0f);

            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
    }

    public sealed class MoveByInput_EntityComponent : IEntityComponentUpdater
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField, MinValue(0f)] private float _movementSpeed;
        [SerializeField, MinValue(0f)] private float _rotationSpeed;
        [SerializeField] private InputActionReference _moveAxis;

        public bool IsActive { get; set; }
        public bool IsMoving => _motion.sqrMagnitude > 0.01f;
        private Vector3 _motion;

        public void Update(MonoBehaviour owner)
        {
            var input = _moveAxis.action.ReadValue<Vector2>().normalized;
            _motion.x = input.x;
            _motion.z = input.y;

            _characterController.Move(_movementSpeed * Time.deltaTime * _motion);

            if (IsMoving)
            {
                owner.transform.rotation = Quaternion.Slerp(
                    owner.transform.rotation,
                    Quaternion.LookRotation(_motion),
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    public sealed class InteractionBrowser_EntityComponent : IEntityComponentInitializer, 
        IEntityComponentUpdater,
        IEntityComponentCleaner
    {
        public bool IsActive { get; set; }
        private IInteractable _currentInteractable;
        
        [SerializeField] private Raycast_CustomCollision _raycast;
        [SerializeField] private InputActionReference _interactInput;

        private IInteractable CurrentInteractable
        {
            get => _currentInteractable;
            set
            {
                if (_currentInteractable == value) return;
                _currentInteractable = value;

                EventBus.Raise(new InteractionFound_GameEvent(CurrentInteractable));
            }
        }

        public void Initialize(MonoBehaviour owner)
        {
            _interactInput.action.Enable();
            _interactInput.action.performed += TryInteract;
        }

        public void Clear(MonoBehaviour owner)
        {
            _interactInput.action.Disable();
            _interactInput.action.performed -= TryInteract;
        }

        public void Update(MonoBehaviour owner)
        {
            if (_raycast.GetCollisions(transform: owner.transform, amount: 1) is { Length: > 0 } collisions && 
                collisions.First() is { } firstCollision)
            {
                if (firstCollision.TryGetComponent<IInteractable>(out var interactable) &&
                    interactable.IsInteractable)
                {
                    CurrentInteractable = interactable;
                    return;
                }
            }

            CurrentInteractable = null;
        }

        private void TryInteract(InputAction.CallbackContext _)
        {
            if (CurrentInteractable is not { IsInteractable: true }) return;
            
            CurrentInteractable.Interact();
            
            EventBus.Raise(new InteractionStart_GameEvent(CurrentInteractable));
            CurrentInteractable = null;
        }
    }
}
