using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public event Action<int> OnTryUnlockRoom;
    private Dictionary<int, Door> _doors = new();
    
    public void EnableDoor(GameProgressState state)
    {
        if (state.status == GameProgressState.Status.TALK)
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
    
    public void UnlockRoom(int roomId)
    {
        if (_doors.TryGetValue(roomId, out var door))
        {
            door.Unlock();
        }
    }
}
