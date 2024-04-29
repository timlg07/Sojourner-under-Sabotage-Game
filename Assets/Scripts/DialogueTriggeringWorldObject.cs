using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggeringWorldObject : MonoBehaviour
{
    
    [SerializeField, TextArea] private List<String> dialogue = new();
    private InteractableWorldObject _interactableWorldObject;

    public void Start()
    {
        if (dialogue.Count > 0)
        {
            _interactableWorldObject = GetComponent<InteractableWorldObject>();
            _interactableWorldObject.onPlayerInteract.AddListener(TriggerDialogue);
            _interactableWorldObject.interactionIndicator.enabled = true;
            _interactableWorldObject.IsEnabled = true;
        }
    }

    private void TriggerDialogue()
    {
        if (dialogue.Count > 0)
        {
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
