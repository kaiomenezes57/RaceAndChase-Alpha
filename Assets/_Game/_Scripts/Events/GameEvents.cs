using Game.Entity;
using Game.Entity.Combat;

namespace Game.Events
{
    public interface IGameEvent { }
    public sealed class TakeDamage_GameEvent : IGameEvent
    {
        public DamageData DamageData { get; private set; }

        public TakeDamage_GameEvent(DamageData damageData)
        {
            DamageData = damageData;
        }
    }

    public sealed class TakeHeal_GameEvent : IGameEvent
    {
        public HealData HealData { get; private set; }

        public TakeHeal_GameEvent(HealData healData)
        {
            HealData = healData;
        }
    }

    public sealed class EntityDie_GameEvent : IGameEvent
    {
        public IAliveEntity AliveEntity { get; private set; }

        public EntityDie_GameEvent(IAliveEntity aliveEntity)
        {
            AliveEntity = aliveEntity;
        }
    }
}
