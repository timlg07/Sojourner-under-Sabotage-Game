using DG.Tweening;
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
        MoveTween = transform.DOMoveY(InitialPosition.y + _height, OpenDuration);
    }
    
    public override void Close()
    {
        base.Close();
        if (IsNotClear()) return;
        
        ScaleTween = transform.DOScaleY(InitialScaleY, CloseDuration);
        MoveTween = transform.DOMoveY(InitialPosition.y, CloseDuration);
    }
}
