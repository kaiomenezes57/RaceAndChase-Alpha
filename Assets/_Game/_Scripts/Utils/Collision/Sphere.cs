using System.Linq;
using UnityEngine;

namespace Game.Utils.Collision
{
    [System.Serializable]
    public sealed class Sphere_CustomCollision : BaseCustomCollision
    {
        public Sphere_CustomCollision(float size, LayerMask? layerMask = null) : base(size, layerMask) { }

        public override Collider[] GetCollisions(Transform transform, int amount = 10)
        {
            var colliders = new Collider[amount];
            var count = Physics.OverlapSphereNonAlloc(transform.position, _size, colliders, _layerMask);

            return colliders.Take(count).ToArray();
        }
    }
}
