using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] private MeshRenderer indicator;
    [SerializeField] private float frequency = .25f;
    
    void Start()
    {
        indicator = GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        Color textColor = indicator.material.color;
        textColor.a =
            Mathf.Clamp(0.2f + Mathf.Abs(Mathf.Sin(2f * Mathf.PI * frequency * Time.time)), 0f,
                1f);
        indicator.material.color = textColor;
    }
}
