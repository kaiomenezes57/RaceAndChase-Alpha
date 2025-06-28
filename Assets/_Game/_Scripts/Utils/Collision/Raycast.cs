using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Utils.Collision
{
    [System.Serializable]
    public sealed class Raycast_CustomCollision : BaseCustomCollision
    {
        public Raycast_CustomCollision(float size, LayerMask? layerMask = null) : base(size, layerMask) { }

        public override Collider[] GetCollisions(Transform transform, int amount = 10)
        {
            var colliders = new List<Collider>();
            var raycastHits = new RaycastHit[amount];
            var count = Physics.RaycastNonAlloc(transform.position, transform.forward, raycastHits, _size, _layerMask);

            for (int i = 0; i < count; i++)
            {
                if (raycastHits[i].collider == null) continue;
                colliders.Add(raycastHits[i].collider);
            }

            return colliders.Take(count).ToArray();
        }
    }
}
