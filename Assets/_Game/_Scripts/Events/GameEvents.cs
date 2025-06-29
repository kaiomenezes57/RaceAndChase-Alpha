using Game.Entity;
using Game.Entity.Combat;
using Game.Entity.Components;
using Game.Entity.Vehicles;
using Game.Interaction;

namespace Game.Events
{
    public interface IGameEvent { }
    public sealed class VehicleEnter_GameEvent : IGameEvent
    {
        public Vehicle Vehicle { get; private set; }
        
        public VehicleEnter_GameEvent(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }
    }

    public sealed class InteractionStart_GameEvent : IGameEvent
    {
        public IInteractable Interactable { get; private set; }

        public InteractionStart_GameEvent(IInteractable interactable)
        {
            Interactable = interactable;
        }
    }

    public sealed class InteractionFound_GameEvent : IGameEvent
    {
        public IInteractable Interactable { get; private set; }

        public InteractionFound_GameEvent(IInteractable interactable)
        {
            Interactable = interactable;
        }
    }

    public sealed class TakeDamage_GameEvent : IGameEvent
    {
        public DamageData DamageData { get; private set; }

        public TakeDamage_GameEvent(DamageData damageData)
        {
            DamageData = damageData;
        }
    }

    public sealed class TakeHeal_GameEvent : IGameEvent
    {
        public HealData HealData { get; private set; }

        public TakeHeal_GameEvent(HealData healData)
        {
            HealData = healData;
        }
    }

    public sealed class EntityDie_GameEvent : IGameEvent
    {
        public IAlive Health { get; private set; }

        public EntityDie_GameEvent(IAlive aliveEntity)
        {
            Health = aliveEntity;
        }
    }
}