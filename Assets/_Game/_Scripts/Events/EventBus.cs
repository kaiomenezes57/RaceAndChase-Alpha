using System;
using System.Collections.Generic;

namespace Game.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public static void Subscribe<T>(Action<T> action) where T : IGameEvent
        {
            var type = typeof(T);

            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();

            _subscribers[type].Add(action);
        }

        public static void UnSubscribe<T>(Action<T> action) where T : IGameEvent
        {
            var type = typeof(T);

            if (_subscribers.TryGetValue(type, out List<Delegate> delegates))
                _subscribers[type].Remove(action);
        }

        public static void Raise<T>(T gameEvent) where T : IGameEvent
        {
            var type = typeof(T);

            if (_subscribers.TryGetValue(type, out List<Delegate> delegates))
            {
                for (int i = 0; i < delegates.Count; i++)
                    (delegates[i] as Action<T>)?.Invoke(gameEvent);
            }
        }
    }
}
