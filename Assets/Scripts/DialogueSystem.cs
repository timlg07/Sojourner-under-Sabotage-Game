using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance => _instance;
    private static DialogueSystem _instance;

    public UnityEvent onHasDialogueToShow;
    public bool HasDialogueToShow => _hasDialogueToShow;
    [SerializeField] private List<DialogueEntry> _dialogueEntries = new();
    private readonly Dictionary<GameProgressMilestone, List<string>> _dialogueMap = new();
    private GameProgressMilestone _currentCondition;
    private bool _wasDialogueShown;
    private bool _hasDialogueToShow = false;

    private static void RegisterInstance(DialogueSystem instance)
    {
        if (_instance == null)
        {
            _instance = instance;
        }
        else
        {
            Debug.LogError("There is already a DialogueSystem instance in the scene!");
        }
    }
    
    public void Awake()
    {
        RegisterInstance(this);
        
        // transform list to map
        _dialogueEntries.ForEach(entry =>
        {
            if (_dialogueMap.ContainsKey(entry.condition))
            {
                Debug.LogError($"Duplicate dialogue entry for condition {entry.condition}!");
            }
            else
            {
                _dialogueMap.Add(
                    entry.condition, 
                    entry.text.Split("\n\n").ToList()
                );
            }
        });
    }
    
    public void Start()
    {
        GameProgress.Instance.onGameProgressChanged.AddListener(StoreDialogue);
    }

    private void StoreDialogue(GameProgressMilestone condition)
    {
        _wasDialogueShown = false;
        _currentCondition = condition;
        
        _hasDialogueToShow = true;
        onHasDialogueToShow?.Invoke();
    }
    
    public void ShowDialogue()
    {
        if (_wasDialogueShown)
        {
            return;
        }
        
        if (_dialogueMap.TryGetValue(_currentCondition, out var dialogue))
        {
            DialogueUI.Instance.ShowDialogue(dialogue);
        }
        else
        {
            Debug.LogError($"No dialogue entry for condition {_currentCondition}!");
        }
        _hasDialogueToShow = false;
    }

    [Serializable]
    public struct DialogueEntry
    {
        public GameProgressMilestone condition;
        [TextArea] public string text;
    }
}