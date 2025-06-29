using Game.Events;
using Game.Interaction;
using Game.Utils.Collision;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Player
{
    public sealed class PlayerInteractionHandler : MonoBehaviour
    {
        [SerializeField] private Raycast_CustomCollision _raycast;
        [SerializeField] private InputActionReference _interactInput;
        private IInteractable _currentInteractable;

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

        private void OnEnable()
        {
            _interactInput.action.Enable();
            _interactInput.action.performed += TryInteract;
        }

        private void OnDisable()
        {
            _interactInput.action.Disable();
            _interactInput.action.performed -= TryInteract;
        }

        private void Update()
        {
            if (_raycast.GetCollisions(transform: transform, amount: 1) is { Length: > 0 } collisions &&
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
