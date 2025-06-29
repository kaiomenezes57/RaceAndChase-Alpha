using Game.Entity.Components;
using Game.Entity.Player;
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
            playerGO.GetComponent<PlayerMovement>().enabled = false;

            car.SetDriver(Vehicle.DriverState.Player);
            CurrentVehicle = car;

            EventBus.Raise(new VehicleEnter_GameEvent(CurrentVehicle));
        }

        public void ExitVehicle()
        {
            if (CurrentVehicle is not { IsMoving: false }) return;
            var playerGO = PlayerGR.Instance.GameObject;

            playerGO.SetActive(true);
            playerGO.transform.SetParent(null);
            playerGO.transform.position = CurrentVehicle.transform.position + (-CurrentVehicle.transform.right * 2f);

            playerGO.GetComponent<PlayerMovement>().enabled = true;

            CurrentVehicle.SetDriver(Vehicle.DriverState.None);
            CurrentVehicle = null;
        }
    }
}
