using UnityEngine;
using DG.Tweening;

public class Alarm : MonoBehaviour
{
    [SerializeField]
    [ContextMenuItem("TriggerAlarm - Item", "TriggerAlarm")]
    private UnityEngine.UI.Image image;

    private void Start()
    {
        image.color = new Color(255, 0, 0, 0);
    }

    [ContextMenu("TriggerAlarm")]
    public void TriggerAlarm()
    {
        TriggerAlarm("Demo");
    }
    
    public void TriggerAlarm(string componentName)
    {
        Debug.Log("Alarm triggered for " + componentName);
        Color targetColor = new Color(255, 0, 0, .5f); 
        float duration = 1f;

        DOTween.To(
            () => image.color.a, 
            x =>
            {
                targetColor.a = x;
                image.color = targetColor;
            }, 
            .5f, 
            duration
        ).SetLoops(-1, LoopType.Yoyo);
    }
}
