using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent<ComponentBehaviour> onMutatedComponentTestsFailed;

    public readonly Dictionary<string, ComponentBehaviour> Components = new();

    public void Start()
    {
        foreach (var c in FindObjectsByType<ComponentBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Components.Add(c.componentName, c);
        }
    }

    public void OnMutatedComponentTestsFailed(string componentName)
    {
        Debug.Log("Mutated component tests failed");
        onMutatedComponentTestsFailed?.Invoke(Components[componentName]);
    }

    [ContextMenu("OnMutatedComponentTestsFailed [Demo]")]
    public void TriggerDemoAlarm()
    {
        OnMutatedComponentTestsFailed("Demo");
    }
}
