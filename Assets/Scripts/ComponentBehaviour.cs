using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class ComponentBehaviour : MonoBehaviour
{
    public string componentName;
    private InteractableWorldObject _interactableWorldObject;
    private bool _wasNeverOpened = true;
    
    private void Start()
    {
        _interactableWorldObject = GetComponent<InteractableWorldObject>();
    }

    public void OpenComponent()
    {
        Debug.Log("Opening component " + componentName);
        BrowserUI.OpenEditorsForComponent(componentName);
        _wasNeverOpened = false;
        FindObjectOfType<BrowserUI>().onEditorCloseEvent.AddListener(EnableComponentInteraction);
    }
    
    public void DisableComponentInteraction()
    {
        _interactableWorldObject.IsEnabled = false;
        _interactableWorldObject.interactionIndicator.Hide();
    }
    
    public void EnableComponentInteraction()
    {
        _interactableWorldObject.IsEnabled = true;
        _interactableWorldObject.interactionIndicator.SetVisible(_wasNeverOpened);
    }
    
    public void HighlightInteraction()
    {
        _interactableWorldObject.interactionIndicator.Show();
    }
}
