using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CreativeSpore.RpgMapEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatedTile : MonoBehaviour
{
    [SerializeField] 
    private List<int> tileIds;

    [SerializeField] 
    private int layer = 1;
    
    [SerializeField, Tooltip("Wait time during each animation frame in seconds")] 
    private float interval = .32f;
    
    private int _currentFrame;
    private float _timeSinceLastFrame;

    void Update()
    {
        _timeSinceLastFrame += Time.deltaTime;
        if (_timeSinceLastFrame >= interval)
        {
            _timeSinceLastFrame = 0;
            _currentFrame = (_currentFrame + 1) % tileIds.Count;
            RpgMapHelper.SetAutoTileByPosition(transform.position, tileIds[_currentFrame], layer);
        }
    }
}
