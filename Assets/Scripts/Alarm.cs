using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Alarm : MonoBehaviour
{

    [SerializeField]
    private UnityEngine.UI.Image image;
    
#if UNITY_EDITOR || !UNITY_WEBGL
    private TweenerCore<float, float, FloatOptions> _alarmTween;
#endif
    
    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        image.gameObject.SetActive(false);
#else
        image.color = new Color(255, 0, 0, 0);
#endif
    }

    
    public void TriggerAlarm(ComponentBehaviour component)
    {
        Debug.Log("Alarm triggered for " + component.componentName);
#if !UNITY_EDITOR && UNITY_WEBGL
        BrowserUI.DoToggleAlarm(true);
#else
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
#endif
    }
    
    [ContextMenu("Stop Alarm")]
    public void StopAlarm()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        BrowserUI.DoToggleAlarm(false);
#else
        _alarmTween.Kill();
        image.color = new Color(255, 0, 0, 0);
#endif
    }
}
