using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField] private int roomId;
    private void Start()
    {
        EventManager.Instance.onGameProgressionChanged.AddListener(HandleFirstGameProgressionChanged);
    }

    private void HandleFirstGameProgressionChanged(GameProgressState gps)
    {
        if (gps.room > 1 && gps.room == roomId)
        {
            var target = transform.position;
            var player = FindObjectOfType<PlayerController>();
            var companion = FindObjectOfType<CustomFollowerAI>();
            player.transform.position = target;
            companion.transform.position = target;
        }
        EventManager.Instance.onGameProgressionChanged.RemoveListener(HandleFirstGameProgressionChanged);
    }
}
