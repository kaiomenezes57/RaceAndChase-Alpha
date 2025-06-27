using UnityEngine;

namespace Game.StateMachine
{
    public interface IStateMachine
    {
        MonoBehaviour MonoBehaviour { get; }
        IState CurrentState { get; }
        void Change(IState state);
    }

    public abstract class BaseStateMachine : MonoBehaviour, IStateMachine
    {
        public MonoBehaviour MonoBehaviour => this;
        public IState CurrentState { get; private set; }
        protected abstract IState FirstState { get; }

        private void Start()
        {
            Change(FirstState);
        }

        private void Update()
        {
            (CurrentState as IStateUpdate)?.Update(this);
        }

        public void Change(IState state)
        {
            (CurrentState as IStateExit)?.Exit(this);
            CurrentState = state;

            (CurrentState as IStateEnter)?.Enter(this);
        }
    }

    #region STATE
    public interface IState { }
    public interface IStateEnter : IState
    {
        void Enter(IStateMachine stateMachine);
    }

    public interface IStateUpdate : IState
    {
        void Update(IStateMachine stateMachine);
    }

    public interface IStateExit : IState
    {
        void Exit(IStateMachine stateMachine);
    }
    #endregion
}
