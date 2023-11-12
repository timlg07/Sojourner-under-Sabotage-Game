using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class VerticalDoor : Door
{
    private float _height;

    public override void Start()
    {
        base.Start();
        var childRenderer = GetComponentInChildren<Renderer>();
        _height = childRenderer.bounds.size.y * 3;
    }

    public override void Open()
    {
        base.Open();
        if (IsLocked) return;
        
        ScaleTween = transform.DOScaleY(0, OpenDuration);
        MoveTween = transform.DOMoveY(InitialPosition.y + _height - .1f, OpenDuration);
    }
    
    public override void Close()
    {
        base.Close();
        if (IsNotClear()) return;
        
        ScaleTween = transform.DOScaleY(1, CloseDuration);
        MoveTween = transform.DOMoveY(InitialPosition.y, CloseDuration);
    }
}
