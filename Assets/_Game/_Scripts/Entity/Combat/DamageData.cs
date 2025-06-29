using Game.Entity.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Combat
{
    public struct DamageData
    {
        public IDamageDealer Dealer { get; private set; }
        public IAlive Target { get; private set; }
        public float Amount { get; private set; }
        public List<IDamageEffect> Effects { get; private set; }

        public DamageData(IDamageDealer dealer, IAlive target, float amount, 
            List<IDamageEffect> effects = null)
        {
            Dealer = dealer;
            Target = target;
            Amount = Mathf.Clamp(amount, 0f, float.MaxValue);

            Effects = effects ?? new List<IDamageEffect>();
        }
    }
}
