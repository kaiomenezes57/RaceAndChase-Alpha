using UnityEngine;
using VContainer;

namespace Game.Utils.GlobalReferences
{
    public abstract class BaseGlobalReference<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }
        [Inject] private readonly T _instance;

        private void Start()
        {
            Instance = _instance;
        }

        private void OnDisable()
        {
            Instance = default;
        }
    }
}
