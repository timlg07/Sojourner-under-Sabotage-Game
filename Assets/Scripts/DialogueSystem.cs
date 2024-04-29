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

    public event Action<List<String>> OnShowDialogue; // used to trigger OneJS
    public UnityEvent onHasDialogueToShow;
    public bool HasDialogueToShow => _hasDialogueToShow;
    [FormerlySerializedAs("_dialogueEntries")] [SerializeField] 
    private List<DialogueEntry> dialogueEntries = new();
    private readonly Dictionary<GameProgressState.DialogueCondition, List<string>> _dialogueMap = new();
    private bool _wasDialogueShown;
    private bool _hasDialogueToShow = false;
    private ComponentBehaviour _activateAfterDialogue;

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
        dialogueEntries.ForEach(entry =>
        {
            if (_dialogueMap.ContainsKey(entry.Condition))
            {
                Debug.LogError($"Duplicate dialogue entry for condition {entry.Condition}!");
            }
            else
            {
                _dialogueMap.Add(
                    entry.Condition, 
                    entry.text.Split("\n\n").ToList()
                );
            }
        });
    }
    
    public void Start()
    {
        EventManager.Instance.onGameProgressionChanged.AddListener(UpdateCurrentGameProgression);
        EventManager.Instance.onComponentDestroyed.AddListener(DisableInteraction);
        EventManager.Instance.onMutatedComponentTestsFailed.AddListener(DisableInteraction);
    }

    private void UpdateCurrentGameProgression(GameProgressState condition)
    {
        var hasDialogueToShow = _dialogueMap.ContainsKey(new GameProgressState.DialogueCondition(condition));
        if (hasDialogueToShow)
        {
            _wasDialogueShown = false;
            _hasDialogueToShow = true;
            onHasDialogueToShow?.Invoke();
        } 
        else if (condition.status is GameProgressState.Status.MUTATED or GameProgressState.Status.DESTROYED)
        {
            // no dialogue to show, but the component interaction might have been already disabled
            EnableInteraction();
        }
    }
    
    public void ShowDialogue()
    {
        if (_wasDialogueShown)
        {
            return;
        }

        var currentCondition = new GameProgressState.DialogueCondition(GameProgressState.CurrentState);
        if (_dialogueMap.TryGetValue(currentCondition, out var dialogue))
        {
            OnShowDialogue?.Invoke(dialogue);
        }
        else
        {
            Debug.LogError($"No dialogue entry for condition {GameProgressState.CurrentState}!");
        }
        _hasDialogueToShow = false;
    }

    public void DisableInteraction(ComponentBehaviour c)
    {
        var condition = new GameProgressState.DialogueCondition(GameProgressState.CurrentState);
        if (_dialogueMap.ContainsKey(condition) && condition.status is GameProgressState.Status.MUTATED or GameProgressState.Status.DESTROYED)
        {
            _activateAfterDialogue = c;
            _activateAfterDialogue.DisableComponentInteraction();
        }
    }
    
    // called from OneJS
    public void EnableInteraction()
    {
        if (_activateAfterDialogue == null) return;
        _activateAfterDialogue.EnableComponentInteraction();
        _activateAfterDialogue = null;
    }

    [Serializable]
    public struct DialogueEntry
    {
        public GameProgressState.DialogueCondition Condition;
        [TextArea] public string text;
    }

    public void PlayExternalDialogue(List<string> dialogue)
    {
        OnShowDialogue?.Invoke(dialogue);
    }
}