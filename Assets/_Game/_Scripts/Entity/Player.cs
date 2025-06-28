using UnityEngine;

namespace Game.Entity
{
    public interface IPlayer
    {
        IAliveEntity AliveEntity { get; }
        GameObject GameObject { get; }
    }

    public sealed class Player : BaseAliveEntity, IPlayer
    {
        public IAliveEntity AliveEntity => this;
        public GameObject GameObject => gameObject;

        protected override void Die()
        {
            base.Die();
#if UNITY_EDITOR
            Debug.Log("You died!");
#endif
        }
    }
}