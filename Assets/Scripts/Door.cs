using System;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
    private TweenerCore<Vector3, Vector3, VectorOptions> _scaleTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> _moveTween;
    private InteractableWorldObject _iwo;

    void Start()
    {
        var childRenderer = GetComponentInChildren<Renderer>();
        _height = childRenderer.bounds.size.y * 3;
        
        _initialPosition = transform.position;
        _initialTileId = RpgMapHelper.GetAutoTileByPosition(_initialPosition, _blockLayer).Id;
        
        _iwo = GetComponent<InteractableWorldObject>();
    }

    public void Open()
    {
        if (_isLocked) return;
        
        AbortTweens();
        _scaleTween = transform.DOScaleY(0, openDuration);
        _moveTween = transform.DOMoveY(_initialPosition.y + _height - .1f, openDuration);
        
        RpgMapHelper.SetAutoTileByPosition(_initialPosition, 0, _blockLayer);
    }
    
    public void Close()
    {
        if (IsNotClear()) return;
        
        AbortTweens();
        _scaleTween = transform.DOScaleY(1, closeDuration);
        _moveTween = transform.DOMoveY(_initialPosition.y, closeDuration);
        
        if (_isLocked) RpgMapHelper.SetAutoTileByPosition(_initialPosition, _initialTileId, _blockLayer);
    }

    private bool IsNotClear()
    {
        return _iwo.IsPetInRange || _iwo.IsPlayerInRange;
    }

    public void Unlock()
    {
        _isLocked = false;
        Open();
    }

    private void AbortTweens()
    {
        if (_scaleTween != null && _scaleTween.IsPlaying()) _scaleTween.Kill();
        if (_moveTween != null && _moveTween.IsPlaying()) _moveTween.Kill();
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
