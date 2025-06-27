using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Components
{
    public interface IEntityComponentsHolder
    {
        void AddEntityComponent<T>(T component) where T : IEntityComponent;
        T GetEntityComponent<T>() where T : IEntityComponent;
        void RemoveComponents<T>() where T : IEntityComponent;
    }

    public sealed partial class EntityComponentsHolder : MonoBehaviour
    {
        [OdinSerialize, SerializeReference] private List<IEntityComponent> _components = new();

        private void OnEnable()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                (_components[i] as IEntityComponentInitializer)?.Initialize(this);
                _components[i].IsActive = true;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (!_components[i].IsActive) continue;
                (_components[i] as IEntityComponentUpdater)?.Update(this);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                (_components[i] as IEntityComponentCleaner)?.Clear(this);
                _components[i].IsActive = false;
            }
        }
    }

    public sealed partial class EntityComponentsHolder : IEntityComponentsHolder
    {
        public void AddEntityComponent<T>(T component) where T : IEntityComponent
        {
            _components.Add(component);
        }

        public T GetEntityComponent<T>() where T : IEntityComponent
        {
            return (T)_components.Find(c => c.GetType() == typeof(T));
        }

        public void RemoveComponents<T>() where T : IEntityComponent
        {
            _components.RemoveAll(c => c.GetType() == typeof(T));
        }
    }
}
