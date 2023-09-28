using UnityEngine;
using System.Runtime.InteropServices;

public class BrowserUI : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void _ShowMessage(string message);
#endif

    public void Start()
    {
#if UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif
    }
    
    public static void ShowMessage()
    {
#if UNITY_WEBGL
        _ShowMessage("Hello World");
#else
        Debug.Log("Not supported on this platform (needs to be the WebGL export)");  
#endif
    }
}
