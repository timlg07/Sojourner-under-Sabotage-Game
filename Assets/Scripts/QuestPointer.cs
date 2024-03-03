using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] public GameObject target;
    [SerializeField] private RectTransform pointerRectTransform;
    [SerializeField] private Sprite pointerSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Image pointerImage;
    [SerializeField] private float borderSize = 100f;
    
    public void Hide()
    {
        pointerImage.gameObject.SetActive(false);
    }
    
    public void Show(GameObject pTarget)
    {
        target = pTarget;
        pointerImage.gameObject.SetActive(true);
    }
    
    public void Show(ComponentBehaviour pTarget)
    {
        Show(pTarget.gameObject);
    }
    
    void Start()
    {
        if (pointerRectTransform == null) pointerRectTransform = GetComponent<RectTransform>();
        if (pointerImage == null) pointerImage = GetComponent<Image>();
        
        Hide();
    }
    
    void Update()
    {
        Vector3 toPosition = target.transform.position;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(toPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize ||
                           targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;
        pointerImage.sprite = isOffScreen ? pointerSprite : crossSprite;
        if (isOffScreen)
        {
            RotatePointerTowardsTarget(toPosition);
        }
        else
        {
            pointerRectTransform.localEulerAngles = Vector3.zero;
        }

        Vector3 cappedTargetScreenPosition = new Vector3(
            Mathf.Clamp(targetPositionScreenPoint.x, borderSize, Screen.width - borderSize),
            Mathf.Clamp(targetPositionScreenPoint.y, borderSize, Screen.height - borderSize),
            targetPositionScreenPoint.z
        );
            
        Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
        pointerRectTransform.position = pointerWorldPosition;
        pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
    }
    
    private void RotatePointerTowardsTarget(Vector3 toPosition)
    {
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
