using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggeringWorldObject : MonoBehaviour
{
    [SerializeField] private int roomId;
    [SerializeField, TextArea] private List<String> dialogue = new();
    private InteractableWorldObject _interactableWorldObject;

    public void Start()
    {
        _interactableWorldObject = GetComponent<InteractableWorldObject>();
        _interactableWorldObject.interactionIndicator.Hide();
        _interactableWorldObject.IsEnabled = false;
        
        if (dialogue.Count > 0)
        {
            _interactableWorldObject.onPlayerInteract.AddListener(TriggerDialogue);
        }
        
        EventManager.Instance.onGameProgressionChanged.AddListener(GameProgressionChanged);
    }

    private void GameProgressionChanged(GameProgressState p)
    {
        if (p.status == GameProgressState.Status.TESTS_ACTIVE && p.room == roomId)
        {
            _interactableWorldObject.interactionIndicator.Show();
            _interactableWorldObject.IsEnabled = true;
        }
    }

    private void TriggerDialogue()
    {
        if (dialogue.Count > 0)
        {
            if (DialogueSystem.Instance.IsDialoguePlaying)
            {
                _interactableWorldObject.IsEnabled = true;
                return;
            }
            
            DialogueSystem.Instance.PlayExternalDialogue(dialogue);
            var ii = _interactableWorldObject.interactionIndicator;
            if (ii != null)
            {
                _interactableWorldObject.interactionIndicator = null;
                Destroy(ii.gameObject);
            }
        }
    }
}
