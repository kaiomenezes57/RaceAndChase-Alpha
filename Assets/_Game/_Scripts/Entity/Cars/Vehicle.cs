using Game.Entity.Components;
using Game.Interaction;
using Game.Utils.GlobalReferences;
using UnityEngine;
using VContainer;

namespace Game.Entity.Vehicles
{
    public sealed class Vehicle : BaseAliveEntity, IInteractable
    {
        public enum DriverState { None = 0, NPC = 1, Player = 2, }
        public bool IsInteractable => _driverState != DriverState.Player;
        public bool IsMoving => _driveByInput.IsMoving;
        
        [Inject] private readonly IPlayerVehicleManager _manager;
        private DriveByInput_EntityComponent _driveByInput;
        private DriverState _driverState;

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
            Debug.LogError($"There is no car drive by input component in vehicle.cs.", gameObject);
#endif
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _manager.ExitVehicle();
        }
#endif

        public void SetDriver(DriverState driver)
        {
            _driveByInput.IsActive = driver == DriverState.Player;
            _driverState = driver;
        }

        public void Interact()
        {
            _manager.EnterVehicle(this);
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
