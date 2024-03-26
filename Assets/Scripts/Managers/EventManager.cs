using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingleTon<EventManager>
{
    private static readonly Dictionary<Type, List<EventListenerBase>> _subscribersList
        = new Dictionary<Type, List<EventListenerBase>>();

    public static void Subscribe<GameEvents>(EventListener<GameEvents> listener) where GameEvents : struct
    {
        Type eventType = typeof(GameEvents);

        if (!_subscribersList.ContainsKey(eventType))
        {
            _subscribersList[eventType] = new List<EventListenerBase>();
        }

        if (!SubscriptionExists(eventType, listener))
        {
            _subscribersList[eventType].Add(listener);
        }
    }

    public static void Unsubscribe<GameEvents>(EventListener<GameEvents> listener) where GameEvents : struct
    {
        Type eventType = typeof(GameEvents);

        if (!_subscribersList.ContainsKey(eventType))
            return;

        List<EventListenerBase> subscriberList = _subscribersList[eventType];

        for (int i = subscriberList.Count - 1; i >= 0; i--)
        {
            if (subscriberList[i] == listener)
            {
                subscriberList.Remove(subscriberList[i]);

                if (subscriberList.Count == 0)
                {
                    _subscribersList.Remove(eventType);
                }

                return;
            }
        }
    }

    private static bool SubscriptionExists(Type type, EventListenerBase receiver)
    {
        List<EventListenerBase> receivers;
        if (!_subscribersList.TryGetValue(type, out receivers)) return false;

        bool exists = false;
        for (int i = receivers.Count - 1; i >= 0; i--)
        {
            if (receivers[i] == receiver)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }

    public static void TriggerEvent<GameEvents>(GameEvents events) where GameEvents : struct
    {
        Type eventType = typeof(GameEvents);
        List<EventListenerBase> list;
        if (!_subscribersList.TryGetValue(eventType, out list))
        {
            return;
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            (list[i] as EventListener<GameEvents>).OnEvent(events);
        }
    }
}
public static class GameEventsRegister
{
    public delegate void Delegate<T>(T eventType);

    public static void EventStartingListening<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
        EventManager.Subscribe<EventType>(caller);
    }

    public static void EventStopListening<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
        EventManager.Unsubscribe<EventType>(caller);
    }
}

public interface EventListenerBase { }

public interface EventListener<T> : EventListenerBase
{
    void OnEvent(T eventType);
}