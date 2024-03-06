using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public event Action<int> OnTryUnlockRoom;
    private Dictionary<int, Door> _doors = new();

    public void TryUnlockRoom(int roomId, Door door)
    {
        _doors[roomId] = door;
        OnTryUnlockRoom?.Invoke(roomId);
    }
    
    public void UnlockRoom(int roomId)
    {
        if (_doors.TryGetValue(roomId, out var door))
        {
            door.Unlock();
        }
    }
}
