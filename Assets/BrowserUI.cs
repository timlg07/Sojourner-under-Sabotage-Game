using UnityEngine;
using System.Runtime.InteropServices;

public class BrowserUI : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OpenEditors(string componentName);
#endif

    public void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    UnityEngine.WebGLInput.captureAllKeyboardInput = false;
#endif
    }
    
    public static void OpenEditorsForComponent(string componentName)
    {
#if UNITY_WEBGL
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
    }
}
