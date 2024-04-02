using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent<ComponentBehaviour> onMutatedComponentTestsFailed;
    public UnityEvent<ComponentBehaviour> onComponentDestroyed;
    public UnityEvent<GameProgressState> onGameProgressionChanged;
    public UnityEvent<ComponentBehaviour> onComponentFixed;

    public readonly Dictionary<string, ComponentBehaviour> Components = new();

    private const string DemoComponentName = "Demo";
    [SerializeField, TextArea] private string OnGameProgressionChangedJson = "{\"id\":1,\"room\":1,\"componentName\":\"Demo\",\"stage\":1,\"status\":\"TEST\"}";

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
        OnMutatedComponentTestsFailed(DemoComponentName);
    }
    
    public void OnComponentDestroyed(string componentName)
    {
        Debug.Log("Component "+componentName+" destroyed");
        onComponentDestroyed?.Invoke(Components[componentName]);
    }
    
    [ContextMenu("OnComponentDestroyed [Demo]")]
    public void TriggerDemoComponentDestroyed()
    {
        OnComponentDestroyed(DemoComponentName);
    }
    
    public void OnGameProgressionChanged(string json)
    {
        var unityJson = GameProgressState.ReplaceStatusStringWithInt(json);
        var gameProgressState = JsonUtility.FromJson<GameProgressState>(unityJson);
        GameProgressState.CurrentState = gameProgressState;
        onGameProgressionChanged?.Invoke(gameProgressState);
        
        if (gameProgressState.status == GameProgressState.Status.TEST)
        {
            Components[gameProgressState.componentName].EnableComponentInteraction();
            Debug.Log("Component "+gameProgressState.componentName+" enabled");
        }
    }
    
    [ContextMenu("OnGameProgressionChanged [JSON]")]
    public void TriggerGameProgressionChanged()
    {
        OnGameProgressionChanged(OnGameProgressionChangedJson);
    }
    
    public void OnComponentFixed(string componentName)
    {
        onComponentFixed?.Invoke(Components[componentName]);
    }
}
