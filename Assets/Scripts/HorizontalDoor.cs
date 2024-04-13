using DG.Tweening;
using UnityEngine;

public class HorizontalDoor : Door
{
    private float _width;

    public override void Start()
    {
        base.Start();
        var childRenderer = GetComponentInChildren<Renderer>();
        _width = childRenderer.bounds.size.x;
    }

    public override void Open()
    {
        base.Open();
        if (IsLocked) return;
        
        ScaleTween = transform.DOScaleX(0, OpenDuration);
        MoveTween = transform.DOMoveX(InitialPosition.x - _width / 2, OpenDuration);
    }
    
    public override void Close()
    {
        base.Close();
        if (IsNotClear()) return;
        
        ScaleTween = transform.DOScaleX(InitialScaleY, CloseDuration);
        MoveTween = transform.DOMoveX(InitialPosition.x, CloseDuration);
    }
}
