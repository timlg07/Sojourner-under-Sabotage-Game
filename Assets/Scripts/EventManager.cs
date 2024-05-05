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

    public static EventManager Instance => _instance;
    private static EventManager _instance;
    public readonly Dictionary<string, ComponentBehaviour> Components = new();

    private const string DemoComponentName = "Demo";
    [SerializeField, TextArea] private string OnGameProgressionChangedJson = "{\"id\":1,\"room\":1,\"componentName\":\"Demo\",\"stage\":1,\"status\":\"TEST\"}";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("There is already an EventManager instance in the scene!");
        }
    }

    public void Start()
    {
        foreach (var c in FindObjectsByType<ComponentBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Components.Add(c.componentName, c);
        }
        
        // Send event to server to let it know that unity is ready to receive game progression updtaes
        StompEventDelegation.OnGameStarted();
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
    
    [ContextMenu("OnComponentFixed [Demo]")]
    public void TriggerDemoComponentFixed()
    {
        OnComponentFixed(DemoComponentName);
    }
    
    public void OnGameProgressionChanged(string json)
    {
        var unityJson = GameProgressState.ReplaceStatusStringWithInt(json);
        var gameProgressState = JsonUtility.FromJson<GameProgressState>(unityJson);
        var component = Components[gameProgressState.componentName];
        GameProgressState.CurrentState = gameProgressState;
        onGameProgressionChanged?.Invoke(gameProgressState);
        component.HandleGameProgressionChanged(gameProgressState);
    }
    
    [ContextMenu("OnGameProgressionChanged [JSON]")]
    public void TriggerGameProgressionChanged()
    {
        OnGameProgressionChanged(OnGameProgressionChangedJson);
    }
    
    public void OnComponentFixed(string componentName)
    {
        var c = Components[componentName];
        c.HandleComponentFixed();
        onComponentFixed?.Invoke(c);
    }
}
