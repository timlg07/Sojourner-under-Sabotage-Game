using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CreativeSpore.RpgMapEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatedTile : MonoBehaviour
{
    [SerializeField] private List<int> tileIds;
    
    [SerializeField] private List<int> tileIdsDestroyed;

    [SerializeField] private int layer = 1;
    
    [SerializeField, Tooltip("Wait time during each animation frame in seconds")] 
    private float interval = .32f;

    [SerializeField] private int roomId;
    
    private int _currentFrame;
    private float _timeSinceLastFrame;
    private bool _isDestroyed;

    public void Start()
    {
        EventManager.Instance.onGameProgressionChanged.AddListener(GameProgressionChanged);
    }

    private void GameProgressionChanged(GameProgressState gps)
    {
        var destroy = gps.status is GameProgressState.Status.DESTROYED or GameProgressState.Status.MUTATED;
        var fixedComp = gps.status is GameProgressState.Status.DEBUGGING;
        
        if (gps.room == roomId && (destroy || fixedComp))
        {
            Debug.Log($"Room {roomId} is {gps.status} -> {gameObject.name} plays destroyed animation: {destroy}");
            _isDestroyed = destroy;
            _currentFrame = 0;
            _timeSinceLastFrame = 0;
        }
    }

    void Update()
    {
        var tileIdsCurrent = _isDestroyed && tileIdsDestroyed.Count > 0 ? tileIdsDestroyed : tileIds;
        if (tileIdsCurrent.Count < 2) return;
        
        _timeSinceLastFrame += Time.deltaTime;
        if (_timeSinceLastFrame >= interval)
        {
            _timeSinceLastFrame = 0;
            _currentFrame = (_currentFrame + 1) % tileIdsCurrent.Count;
            RpgMapHelper.SetAutoTileByPosition(transform.position, tileIdsCurrent[_currentFrame], layer);
        }
    }
}
