using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BrowserUI : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OpenEditors(string componentName);
#endif
    
    public UnityEvent onEditorCloseEvent;

    public void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    UnityEngine.WebGLInput.captureAllKeyboardInput = false;
#endif
    }
    
    public static void OpenEditorsForComponent(string componentName)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenEditors(componentName);
        Time.timeScale = 0;
#else
        Debug.Log("Not supported on this platform (needs to be the WebGL export)");
#endif
    }

    public void OnEditorClose()
    {
        Debug.Log("Editor closed");
        Time.timeScale = 1;
        onEditorCloseEvent?.Invoke();
    }
}
