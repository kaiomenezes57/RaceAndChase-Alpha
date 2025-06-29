using Game.Entity.Components;
using UnityEngine;

namespace Game.Entity.Vehicles
{
    public sealed class VehicleHealth : BaseHealth
    {
        protected override void Die()
        {
            base.Die();
#if UNITY_EDITOR
            Debug.Log("Vehicle has been exploded.");
#endif
        }
    }
}
