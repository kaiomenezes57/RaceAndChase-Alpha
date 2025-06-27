using Game.Entity;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.VContainer
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<IPlayer>();
        }

        private void AddGameObjectsScoped<TConcrete>() where TConcrete : MonoBehaviour
        {
            autoInjectGameObjects ??= new List<GameObject>();

            foreach (var component in GetComponentsInChildren<TConcrete>(true))
                autoInjectGameObjects.Add(component.gameObject);
        }
    }
}
