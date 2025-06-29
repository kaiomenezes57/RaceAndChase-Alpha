using Game.Entity.Components;
using UnityEngine;

namespace Game.Entity.Combat
{
    public struct HealData
    {
        public IHealDealer Dealer { get; private set; }
        public IAlive Target { get; private set; }
        public float Amount { get; private set; }

        public HealData(IHealDealer dealer, IAlive target, float amount)
        {
            Dealer = dealer;
            Target = target;
            Amount = Mathf.Clamp(amount, 0f, float.MaxValue);
        }
    }
}
