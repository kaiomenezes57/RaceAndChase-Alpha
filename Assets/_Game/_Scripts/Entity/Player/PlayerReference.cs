using UnityEngine;

namespace Game.Entity.Player
{
    public interface IPlayerReference
    {
        GameObject GameObject { get; }
    }

    public sealed class PlayerReference : MonoBehaviour, IPlayerReference
    {
        public GameObject GameObject => gameObject;
    }
}