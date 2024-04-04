using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorManager : MonoBehaviour
{
    public event Action<int> OnTryUnlockRoom; // used to trigger OneJS
    private Dictionary<int, Door> _doors = new();

    public void EnableDoor(GameProgressState state)
    {
        if (state.status == GameProgressState.Status.DOOR)
        {
            _doors[state.room].Enable();
        }
    }
    
    public void Register(int roomId, Door door)
    {
        _doors.Add(roomId, door);
    }

    public void TryUnlockRoom(int roomId)
    {
        OnTryUnlockRoom?.Invoke(roomId);
    }
    
    // called from OneJS after mini-game completed
    public void UnlockRoom(int roomId)
    {
        if (_doors.TryGetValue(roomId, out var door))
        {
            door.Unlock();
        }
    }
}
