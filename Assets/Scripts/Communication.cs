using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The hub for scripts to communicate events
/// Instead of scripts referencing eachother, this is the project-wide hub
/// </summary>
public static class Communication
{
    private static Dictionary<Type, Delegate> listeners = new();

    public static void Listen<T>(Action<T> listener, MonoBehaviour attachedListener = null)
    {
        Type eventType = typeof(T);

        if (listeners.ContainsKey(eventType))
            listeners[eventType] = (Action<T>)listeners[eventType] + listener; // Questionable tactics, effective nonetheless
        else
            listeners.Add(eventType, listener);

        // Automatically remove listener when script is destroyed
        if (attachedListener)
            attachedListener.destroyCancellationToken.Register(() => Unlisten(listener));
    }

    public static void Unlisten<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (listeners.ContainsKey(eventType))
            listeners[eventType] = (Action<T>)listeners[eventType] - listener;
    }

    public static void Call<T>(T parameter)
    {
        Type eventType = typeof(T);

        if (listeners.ContainsKey(eventType))
            ((Action<T>)listeners[eventType])?.Invoke(parameter);
        else
            Debug.LogWarning($"Communication: No listeners set to {typeof(T)}");
    }
}