using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteropTest : MonoBehaviour
{
    
    public float Progress => _progress;

    public event Action<float> OnProgressChanged;

    float _progress = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _progress++;
        OnProgressChanged?.Invoke(_progress);
    }
    
    public void OpenWebView()
    {
        BrowserUI.OpenEditorsForComponent("Demo");
    }
}
