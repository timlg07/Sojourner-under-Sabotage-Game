using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance => _instance;
    private static GameProgress _instance;
    
    public UnityEvent<GameProgressMilestone> onGameProgressChanged = new();
    
    private bool _introductionShown = false;

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
    }
    
    void Update()
    {
        if (!_introductionShown)
        {
            onGameProgressChanged?.Invoke(GameProgressMilestone.Introduction);
            _introductionShown = true;
        }
    }
}