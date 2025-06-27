using UnityEngine;

namespace Game.Entity
{
    public interface IPlayer
    {
        IAliveEntity AliveEntity { get; }
    }

    public sealed class Player : BaseAliveEntity, IPlayer
    {
        public IAliveEntity AliveEntity => this;

        protected override void Die()
        {
            base.Die();
#if UNITY_EDITOR
            Debug.Log("You died!");
#endif
        }
    }
}