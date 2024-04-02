using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Alarm : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;
    private TweenerCore<float, float, FloatOptions> _alarmTween;

    private void Start()
    {
        image.color = new Color(255, 0, 0, 0);
    }
    
    public void TriggerAlarm(ComponentBehaviour component)
    {
        Debug.Log("Alarm triggered for " + component.componentName);
        Color targetColor = new Color(255, 0, 0, .35f); 
        float duration = 1f;

        _alarmTween = DOTween.To(
            () => image.color.a, 
            x =>
            {
                targetColor.a = x;
                image.color = targetColor;
            }, 
            .35f, 
            duration
        ).SetLoops(-1, LoopType.Yoyo);
    }
    
    [ContextMenu("Stop Alarm")]
    public void StopAlarm()
    {
        _alarmTween.Kill();
        image.color = new Color(255, 0, 0, 0);
    }
}
