using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance => _instance;
    private static GameProgress _instance;
    
    public UnityEvent<GameProgressMilestone> onGameProgressChanged = new();
    
    private List<GameProgressMilestone> _gameProgress = new();

    private static void RegisterInstance(GameProgress instance)
    {
        if (_instance == null)
        {
            _instance = instance;
        }
        else
        {
            Debug.LogError("There is already a GameProgress instance in the scene!");
        }
    }
    
    void Awake()
    {
        RegisterInstance(this);
        onGameProgressChanged.AddListener(StoreProgress);
    }

    private void StoreProgress(GameProgressMilestone milestone)
    {
        _gameProgress.Add(milestone);
    }

    void Update()
    {
        if (!_gameProgress.Contains(GameProgressMilestone.Introduction))
        {
            onGameProgressChanged.Invoke(GameProgressMilestone.Introduction);
        }
    }
    
    public void DoProgress(GameProgressMilestone milestone)
    {
        onGameProgressChanged.Invoke(milestone);
    }
    
    public void DoProgress(string milestone)
    {
        onGameProgressChanged.Invoke((GameProgressMilestone) System.Enum.Parse(typeof(GameProgressMilestone), milestone));
    }
}