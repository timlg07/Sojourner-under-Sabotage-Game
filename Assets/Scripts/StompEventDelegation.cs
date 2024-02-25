using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class StompEventDelegation : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void SendRoomUnlockedEvent(int roomId);
#endif
    
    public UnityEvent onEditorCloseEvent;

    public static void OnRoomUnlocked(int roomId)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        SendRoomUnlockedEvent(roomId);
#else
        Debug.Log("Not supported on this platform (needs to be the WebGL export)");
#endif
    }
}
