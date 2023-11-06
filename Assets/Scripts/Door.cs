using System;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openDuration = 1f;
    [SerializeField] private float closeDuration = 1f;
    
    private float _height;
    private readonly int _blockLayer = 1;
    private int _initialTileId = -1;
    private Vector3 _initialPosition;
    private bool _isLocked = true;

    void Start()
    {
        var childRenderer = GetComponentInChildren<Renderer>();
        _height = childRenderer.bounds.size.y * 3;
        
        _initialPosition = transform.position;
        _initialTileId = RpgMapHelper.GetAutoTileByPosition(_initialPosition, _blockLayer).Id;
    }

    public void Open()
    {
        if (_isLocked) return;

        transform.DOScaleY(0, openDuration);
        transform.DOMoveY(_initialPosition.y + _height - .1f, openDuration);
        
        RpgMapHelper.SetAutoTileByPosition(_initialPosition, 0, _blockLayer);
    }
    
    public void Close()
    {
        transform.DOScaleY(1, closeDuration);
        transform.DOMoveY(_initialPosition.y, closeDuration);
        
        if (_isLocked) RpgMapHelper.SetAutoTileByPosition(_initialPosition, _initialTileId, _blockLayer);
    }
    
    public void Unlock()
    {
        _isLocked = false;
        Open();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && _initialTileId != -1)
        {
            RpgMapHelper.SetAutoTileByPosition(_initialPosition, _initialTileId, _blockLayer);
        }
#endif
    }
}
