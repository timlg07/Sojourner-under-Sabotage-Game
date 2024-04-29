using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class ComponentBehaviour : MonoBehaviour
{
    public string componentName;
    public int destroyedTileId = 116;
    public int fixedTileId = 84;
    private InteractableWorldObject _interactableWorldObject;
    private bool _wasNeverOpened = true;
    
    private void Start()
    {
        _interactableWorldObject = GetComponent<InteractableWorldObject>();
        _interactableWorldObject.onPlayerInteract.AddListener(() => { _wasNeverOpened = false; });
    }

    public void OpenComponent()
    {
        Debug.Log("Opening component " + componentName);
        BrowserUI.OpenEditorsForComponent(componentName);
        FindObjectOfType<BrowserUI>().onEditorCloseEvent.AddListener(EnableComponentInteraction);
    }
    
    public void DisableComponentInteraction()
    {
        _interactableWorldObject.IsEnabled = false;
        _interactableWorldObject.interactionIndicator.enabled = false;
    }
    
    public void EnableComponentInteraction()
    {
        _interactableWorldObject.IsEnabled = true;
        _interactableWorldObject.interactionIndicator.enabled = _wasNeverOpened;
    }
    
}
