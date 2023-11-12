using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class VerticalDoor : Door
{
    private float _height;
    private TweenerCore<Vector3, Vector3, VectorOptions> _scaleTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> _moveTween;

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
        
        AbortTweens();
        _scaleTween = transform.DOScaleY(0, OpenDuration);
        _moveTween = transform.DOMoveY(InitialPosition.y + _height - .1f, OpenDuration);
    }
    
    public override void Close()
    {
        base.Close();
        if (IsNotClear()) return;
        
        AbortTweens();
        _scaleTween = transform.DOScaleY(1, CloseDuration);
        _moveTween = transform.DOMoveY(InitialPosition.y, CloseDuration);
    }

    private void AbortTweens()
    {
        if (_scaleTween != null && _scaleTween.IsPlaying()) _scaleTween.Kill();
        if (_moveTween != null && _moveTween.IsPlaying()) _moveTween.Kill();
    }
}
