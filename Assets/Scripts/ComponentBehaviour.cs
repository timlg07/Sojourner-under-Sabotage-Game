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

    public void OpenComponent()
    {
        Debug.Log("Opening component " + componentName);
        BrowserUI.OpenEditorsForComponent(componentName);
        FindObjectOfType<BrowserUI>().onEditorCloseEvent.AddListener(() =>
        {
            GetComponent<InteractableWorldObject>().IsEnabled = true;
        });
    }
    
    public void EnableComponentInteraction()
    {
        GetComponent<InteractableWorldObject>().IsEnabled = true;
    }
    
}
