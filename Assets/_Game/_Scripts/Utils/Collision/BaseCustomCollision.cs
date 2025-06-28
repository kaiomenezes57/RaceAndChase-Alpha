using UnityEngine;

namespace Game.Utils.Collision
{
    [System.Serializable]
    public abstract class BaseCustomCollision
    {
        [SerializeField] protected float _size;
        [SerializeField] protected LayerMask _layerMask;

        protected BaseCustomCollision(float size, LayerMask? layerMask = null)
        {
            _layerMask = layerMask ?? ~0;
            _size = size;
        }

        public abstract Collider[] GetCollisions(Transform transform, int amount = 10);
    }
}
