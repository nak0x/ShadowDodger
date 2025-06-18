using System;
using System.Collections.Generic;

namespace Events
{
    public enum EventType
    {
        PlayerDied,
        PlayerRespawned
    }

    public static class EventManager
    {
        private static readonly Dictionary<EventType, Action> EventTable = new();

        public static void Subscribe(EventType type, Action handler)
        {
            if (EventTable.ContainsKey(type))
                EventTable[type] += handler;
            else
                EventTable[type] = handler;
        }

        public static void Unsubscribe(EventType type, Action handler)
        {
            if (EventTable.ContainsKey(type))
                EventTable[type] -= handler;
        }

        public static void Emit(EventType type)
        {
            if (EventTable.TryGetValue(type, out var action))
            {
                action?.Invoke();
            }
        }
    }
}
