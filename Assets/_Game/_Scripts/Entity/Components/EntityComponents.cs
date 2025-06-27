using UnityEngine;

namespace Game.Entity.Components
{
    #region ABSTRACT
    public interface IEntityComponent { }

    public interface IEntityComponentInitializer : IEntityComponent
    {
        void Initialize(MonoBehaviour owner);
    }

    public interface IEntityComponentUpdater : IEntityComponent
    {
        void Update(MonoBehaviour owner);
    }

    public interface IEntityComponentCleaner : IEntityComponent
    {
        void Clear(MonoBehaviour owner);
    }
    #endregion
}
