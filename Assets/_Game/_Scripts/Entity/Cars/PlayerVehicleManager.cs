using Game.Entity.Components;
using Game.Events;
using Game.Utils.GlobalReferences;
using UnityEngine;

namespace Game.Entity.Vehicles
{
    public interface IPlayerVehicleManager
    {
        void EnterVehicle(Vehicle car);
        void ExitVehicle();
    }

    public sealed class PlayerVehicleManager : MonoBehaviour, IPlayerVehicleManager
    {
        public Vehicle CurrentVehicle { get; private set; }

        public void EnterVehicle(Vehicle car)
        {
            if (car == null || CurrentVehicle != null) return;
            var playerGO = PlayerGR.Instance.GameObject;

            playerGO.SetActive(false);
            playerGO.transform.SetParent(car.transform);
            playerGO.transform.localPosition = Vector3.zero;

            //TO DO
            PlayerGR.Instance.GameObject
                .GetComponent<EntityComponentsHolder>()
                .GetEntityComponent<MoveByInput_EntityComponent>().IsActive = false;

            car.SetDriver(Vehicle.DriverState.Player);
            CurrentVehicle = car;

            EventBus.Raise(new VehicleEnter_GameEvent(CurrentVehicle));
        }

        public void ExitVehicle()
        {
            if (CurrentVehicle is not { IsMoving: false }) return;
            var player = PlayerGR.Instance.GameObject;

            player.SetActive(true);
            player.transform.SetParent(null);
            player.transform.position = CurrentVehicle.transform.position + (-CurrentVehicle.transform.right * 2f);

            CurrentVehicle.SetDriver(Vehicle.DriverState.None);
            CurrentVehicle = null;
        }
    }
}
