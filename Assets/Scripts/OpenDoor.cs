using System;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private float height;
    private int blockLayer = 1;
    private int oldTileId = -1;
    private Vector3 oldPosition;
    
    void Start()
    {
        var childRenderer = GetComponentInChildren<Renderer>();
        height = childRenderer.bounds.size.y * 3;
    }

    public void Open()
    {
        oldPosition = transform.position;
        oldTileId = RpgMapHelper.GetAutoTileByPosition(oldPosition, blockLayer).Id;
        
        transform.DOScaleY(0, 1f);
        transform.DOMoveY(oldPosition.y + height - .1f, 1f);
        
        RpgMapHelper.SetAutoTileByPosition(oldPosition, 0, blockLayer);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && oldTileId != -1)
        {
            RpgMapHelper.SetAutoTileByPosition(oldPosition, oldTileId, blockLayer);
        }
#endif
    }
}
