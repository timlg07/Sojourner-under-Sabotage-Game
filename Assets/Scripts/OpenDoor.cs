using System;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private int oldTileId = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        transform.DOScaleY(0, 0.5f);
        var tile = RpgMapHelper.GetAutoTileByPosition(transform.position, 1);
        oldTileId = tile.Id;
        RpgMapHelper.SetAutoTileByPosition(transform.position, 0, 1);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && oldTileId != -1)
        {
            RpgMapHelper.SetAutoTileByPosition(transform.position, oldTileId, 1);
        }
#endif
    }
}
