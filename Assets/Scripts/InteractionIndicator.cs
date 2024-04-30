using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] private MeshRenderer indicator;
    [SerializeField] private float frequency = .25f;
    private bool _isVisible = true;
    
    public void Hide() => _isVisible = false;
    public void Show() => _isVisible = true;
    public void SetVisible(bool visible) => _isVisible = visible;
    
    private void Start()
    {
        indicator = GetComponent<MeshRenderer>();
    }
    
    private void Update()
    {
        Color textColor = indicator.material.color;
        textColor.a = _isVisible 
            ? Mathf.Clamp(0.2f + Mathf.Abs(Mathf.Sin(2f * Mathf.PI * frequency * Time.time)), 0f, 1f) 
            : 0f;
        indicator.material.color = textColor;
    }
}
