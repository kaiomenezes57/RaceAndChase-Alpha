using Game.Events;
using Game.Interaction;
using Game.Utils.Collision;
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

    public interface IEntityComponentCleaner : IEntityComponent
    {
        void Clear(MonoBehaviour owner);
    }
    #endregion

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
