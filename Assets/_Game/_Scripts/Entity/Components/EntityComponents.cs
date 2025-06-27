using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Components
{
    #region ABSTRACT
    public interface IEntityComponent 
    {
        bool IsActive { get; set; }
    }

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

    public sealed class MoveByInput_EntityComponent : IEntityComponentUpdater
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField, MinValue(0f)] private float _movementSpeed;
        [SerializeField] private InputActionReference _moveAxis;

        public bool IsActive { get; set; }
        public bool IsMoving => _motion.sqrMagnitude > 0.01f;
        private Vector3 _motion;

        public void Update(MonoBehaviour owner)
        {
            var input = _moveAxis.action.ReadValue<Vector2>().normalized;
            _motion.x = input.x;
            _motion.z = input.y;

            _characterController.Move(_movementSpeed * Time.deltaTime * _motion);
        }
    }
}
