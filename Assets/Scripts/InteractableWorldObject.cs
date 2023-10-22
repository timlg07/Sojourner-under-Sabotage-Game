using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractableWorldObject : MonoBehaviour
{
    public UnityEvent @event;
    public bool isEnabled;
    public GameObject helpTextObj;
    public float helpTextFlashingFrequency = .25f;
    public float distanceFromPlayerToTrigger = .25f;
    public KeyCode activationKey = KeyCode.Return;
    [Tooltip("If true, the event will only be triggered once.")] public bool reEnableAfterLeave;

    [SerializeField] private bool isActivationKeyPressed;
    [SerializeField] private float distanceToPlayer;

    private PlayerController _player;
    private Renderer _helpTextRenderer;
    private bool _hasHelpText;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _hasHelpText = helpTextObj != null;
        _helpTextRenderer = _hasHelpText ? helpTextObj.GetComponent<Renderer>() : null;
        Reset();
    }

    void Reset()
    {
        isEnabled = true;
    }

    void Update()
    {            
        isActivationKeyPressed = isActivationKeyPressed && Input.GetKey(activationKey) 
                                  || Input.GetKeyDown(activationKey) && Time.timeSinceLevelLoad > 0.5f;
        distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
        
        var isPlayerCloseEnough = distanceToPlayer <= distanceFromPlayerToTrigger;
        if (_hasHelpText) _helpTextRenderer.enabled = isPlayerCloseEnough && isEnabled;
        if (isPlayerCloseEnough)
        {
            if (isActivationKeyPressed && isEnabled)
            {
                @event?.Invoke();
                isEnabled = false;
            }
            else if (_hasHelpText)
            {
                Color textColor = _helpTextRenderer.material.color;
                textColor.a = Mathf.Clamp(0.2f + Mathf.Abs(Mathf.Sin(2f * Mathf.PI * helpTextFlashingFrequency * Time.time)), 0f, 1f);
                _helpTextRenderer.material.color = textColor;
            }
        }
        else if (reEnableAfterLeave)
        {
            isEnabled = true;
        }
    }
}
