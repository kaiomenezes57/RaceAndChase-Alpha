using Game.Entity.Components;
using Game.Interaction;
using Game.Utils.GlobalReferences;
using UnityEngine;

namespace Game.Entity.Cars
{
    public sealed class Car : BaseAliveEntity, IInteractable
    {
        public enum CarDriverState { None = 0, NPC = 1, Player = 2, }
        public bool IsInteractable => _carDriverState != CarDriverState.Player;
        private DriveByInput_EntityComponent _driveByInput;
        private CarDriverState _carDriverState;

        protected override void Start()
        {
            base.Start();

            if (TryGetComponent<EntityComponentsHolder>(out var holder) &&
                holder.GetEntityComponent<DriveByInput_EntityComponent>() is { } driverByInput)
            {
                _driveByInput = driverByInput;
                _driveByInput.IsActive = false;
                return;
            }

#if UNITY_EDITOR
            Debug.LogError($"There is no car drive by input component in car.cs", gameObject);
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }
        }

        private void Enter()
        {
            PlayerGR.Instance.GameObject.SetActive(false);
            PlayerGR.Instance.GameObject.transform.SetParent(transform);

            _driveByInput.IsActive = true;
            _carDriverState = CarDriverState.Player;
        }

        private void Exit()
        {
            if (_driveByInput.IsMoving) return;

            PlayerGR.Instance.GameObject.SetActive(true);
            PlayerGR.Instance.GameObject.transform.SetParent(null);
            PlayerGR.Instance.GameObject.transform.position = transform.position.WithOffset(x: 5f);

            _driveByInput.IsActive = false;
            _carDriverState = CarDriverState.None;
        }

        public void Interact()
        {
            Enter();
        }

        protected override void Die()
        {
            base.Die();
#if UNITY_EDITOR
            Debug.Log($"{gameObject.name} has been exploded.");
#endif
        }
    }
}
