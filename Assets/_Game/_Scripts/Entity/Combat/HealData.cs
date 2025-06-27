using UnityEngine;

namespace Game.Entity.Combat
{
    public struct HealData
    {
        public IHealDealer Dealer { get; private set; }
        public IAliveEntity Target { get; private set; }
        public float Amount { get; private set; }

        public HealData(IHealDealer dealer, IAliveEntity target, float amount)
        {
            Dealer = dealer;
            Target = target;
            Amount = Mathf.Clamp(amount, 0f, float.MaxValue);
        }
    }
}
