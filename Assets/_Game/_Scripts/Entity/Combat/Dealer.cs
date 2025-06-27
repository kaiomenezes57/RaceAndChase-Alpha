using UnityEngine;

namespace Game.Entity.Combat
{
    public interface IDealer
    {
        MonoBehaviour MonoBehaviour { get; }
    }

    public interface IDamageDealer : IDealer
    {
        void DealDamage(DamageData damageData);
    }

    public interface IHealDealer : IDealer
    {
        void DealHeal(HealData healData);
    }
}
