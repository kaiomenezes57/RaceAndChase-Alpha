using Game.Entity.Components;
using UnityEngine;

namespace Game.Entity.Player
{
    public sealed class PlayerHealth : BaseHealth
    {
        protected override void Die()
        {
            base.Die();
#if UNITY_EDITOR
            Debug.Log("You died.");
#endif
        }
    }
}
