using UnityEngine;
using UnityEngine.Events;

namespace Game.Interaction
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        void Interact();
    }

    public abstract class BaseInteraction : MonoBehaviour, IInteractable
    {
        public virtual bool IsInteractable => true;
        public UnityEvent OnInteract;

        public virtual void Interact()
        {
            OnInteract?.Invoke();
        }
    }
}
