using UnityEngine;
using System.Runtime.InteropServices;

public class BrowserUI : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void _ShowMessage(string debug, string test);
#endif

    public void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    UnityEngine.WebGLInput.captureAllKeyboardInput = false;
#endif
    }
    
    public static void ShowMessage()
    {
#if UNITY_WEBGL
        _ShowMessage(
            "public class Demo {\n\tpublic static void main(String[] args) {\n\t\tSystem.out.println(\"Hello World\");\n\t}\n}",
            "public class DemoTest {\n\t@Test\n\tpublic void test() {\n\t\tassertEquals(1,1);\n\t}\n}"
        );
#else
        Debug.Log("Not supported on this platform (needs to be the WebGL export)");
#endif
    }
}
