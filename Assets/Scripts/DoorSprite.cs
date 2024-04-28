using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorSprite : MonoBehaviour
{
    [SerializeField] 
    private Sprite unlockedSprite;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Unlock()
    {
        _spriteRenderer.sprite = unlockedSprite;
    }
}
