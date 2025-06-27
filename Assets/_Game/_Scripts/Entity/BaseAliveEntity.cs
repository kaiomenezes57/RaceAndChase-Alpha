using Game.Entity.Combat;
using Game.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entity
{
    public interface IAliveEntity
    {
        bool IsAlive { get; }
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void Damage(DamageData damageData);
        void Heal(HealData healData);
    }

    public class BaseAliveEntity : MonoBehaviour, IAliveEntity
    {
        public bool IsAlive => CurrentHealth > 0f;
        public float CurrentHealth 
        {
            get => _currentHealth;
            private set
            {
                if (!IsAlive) return;
                _currentHealth = Mathf.Clamp(_currentHealth - value, 0f, MaxHealth);
            }
        }
        [field: SerializeField, MinValue(0f)] public float MaxHealth { get; private set; }
        [SerializeField, ReadOnly] private float _currentHealth;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void Damage(DamageData damageData)
        {
            CurrentHealth -= damageData.Amount;

            if (!IsAlive)
            {
                Die();
                return;
            }

            for (int i = 0; i < damageData.Effects.Count; i++)
                damageData.Effects[i]?.Execute(damageData);
         
            EventBus.Raise(new TakeDamage_GameEvent(damageData));
        }

        public void Heal(HealData healData)
        {
            CurrentHealth += healData.Amount;
            EventBus.Raise(new TakeHeal_GameEvent(healData));
        }

        protected virtual void Die() => EventBus.Raise(new EntityDie_GameEvent(this));
    }
}
