using Game.Interaction;
using UnityEngine;
using VContainer;

namespace Game.Entity.Vehicles
{
    [RequireComponent(typeof(DriveByInput))]
    public sealed class Vehicle : MonoBehaviour, IInteractable
    {
        public enum DriverState { None = 0, NPC = 1, Player = 2, }
        public bool IsInteractable => _driverState != DriverState.Player;
        public bool IsMoving => _driveByInput.IsMoving;
        
        [Inject] private readonly IPlayerVehicleManager _manager;
        private DriveByInput _driveByInput;
        private DriverState _driverState;

        private void Start()
        {
            if (TryGetComponent<DriveByInput>(out var holder))
            {
                _driveByInput = holder;
                _driveByInput.enabled = false;
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
            _driveByInput.enabled = driver == DriverState.Player;
            _driverState = driver;
        }

        public void Interact()
        {
            _manager.EnterVehicle(this);
        }
    }
}
