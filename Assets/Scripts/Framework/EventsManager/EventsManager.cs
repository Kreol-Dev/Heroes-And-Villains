using UnityEngine;
using System.Collections;
using Signals;
using System;
using System.Collections.Generic;
using System.Reflection;

public class EventsManager
{
    Dictionary<Type, BaseSignal> events = new Dictionary<Type, BaseSignal> ();

    public T GetEvent<T> () where T : BaseSignal, new()
    {
        Type eventType = typeof(T);
        BaseSignal e = null;
        events.TryGetValue (eventType, out e);
        if (e == null)
        {
            e = new T ();
        }
        return e as T;
    }

    public EventsManager (params Type[] eventTypes)
    {
        Type baseType = typeof(BaseSignal);
        foreach (var type in eventTypes)
        {
            if (type.IsSubclassOf (baseType) && !events.ContainsKey (type))
                events.Add (type, Activator.CreateInstance (type) as BaseSignal);
        }
    }

}
