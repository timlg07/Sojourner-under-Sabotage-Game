using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorManager : MonoBehaviour
{
    public event Action<int> OnTryUnlockRoom; // used to trigger OneJS
    private Dictionary<int, List<Door>> _doors = new();

    public void EnableDoor(GameProgressState state)
    {
        if (state.status == GameProgressState.Status.DOOR)
        {
            UnlockAllPreviousDoors(state.room);
            _doors[state.room].ForEach(door => door.Enable());
        } 
        else
        {
            UnlockAllPreviousDoors(state.room + 1);
        }
    }
    
    public void Register(int roomId, Door door)
    {
        var doors = _doors.GetValueOrDefault(roomId, new List<Door>());
        doors.Add(door);
        _doors[roomId] = doors;
    }

    public void TryUnlockRoom(int roomId)
    {
        OnTryUnlockRoom?.Invoke(roomId);
    }
    
    // called from OneJS after mini-game completed
    public void UnlockRoom(int roomId)
    {
        if (_doors.TryGetValue(roomId, out var doors))
        {
            doors.ForEach(door => door.Unlock());
        }
    }
    
    private void UnlockAllPreviousDoors(int roomId)
    {
        foreach (var doors in _doors)
        {
            if (doors.Key < roomId)
            {
                doors.Value.ForEach(door =>
                {
                    door.Unlock();
                    door.Close();
                });
            }
        }
    }
}
