using System.Linq;
using UnityEngine;

namespace Game.Utils.Collision
{
    [System.Serializable]
    public sealed class Box_CustomCollision : BaseCustomCollision
    {
        public Box_CustomCollision(float size, LayerMask? layerMask = null) : base(size, layerMask) { }

        public override Collider[] GetCollisions(Transform transform, int amount = 10)
        {
            var colliders = new Collider[amount];
            var count = Physics.OverlapBoxNonAlloc(transform.position, Vector3.zero * _size, colliders);
            
            return colliders.Take(count).ToArray();
        }
    }
}
