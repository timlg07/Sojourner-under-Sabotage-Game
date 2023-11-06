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
    // EVENTS
    public UnityEvent onPlayerInteract;
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerLeave;
    public UnityEvent onPetEnter;

    public UnityEvent onPetLeave;

    // PROPERTIES PUBLIC
    public bool isEnabled;
    public GameObject helpTextObj;
    public float helpTextFlashingFrequency = .25f;
    public float distanceFromPlayerToTrigger = .25f;
    public float distanceFromPetToTrigger = .25f;
    public KeyCode activationKey = KeyCode.Return;

    [Tooltip("If true, the event will only be triggered once.")]
    public bool reEnableAfterLeave;

    // PROPERTIES SERIALIZED
    [SerializeField] private bool isActivationKeyPressed;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private bool fixedPosition;

    // PROPERTIES PRIVATE
    private PlayerController _player;
    private CustomCompanionAI _pet;
    private Renderer _helpTextRenderer;
    private bool _hasHelpText;
    private bool _isPlayerInRange;
    private bool _isPetInRange;
    private Vector3? _position = null;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _pet = FindObjectOfType<CustomCompanionAI>();
        _hasHelpText = helpTextObj != null;
        _helpTextRenderer = _hasHelpText ? helpTextObj.GetComponent<Renderer>() : null;
        Reset();
    }

    void Reset()
    {
        isEnabled = true;
        _isPlayerInRange = false;
        if (fixedPosition)
        {
            _position = transform.position;
        }
    }

    void Update()
    {
        isActivationKeyPressed = isActivationKeyPressed && Input.GetKey(activationKey)
                                 || Input.GetKeyDown(activationKey) && Time.timeSinceLevelLoad > 0.5f;
        var pos = _position ?? transform.position;
        distanceToPlayer = Vector2.Distance(pos, _player.transform.position);
        var distanceToPet = Vector2.Distance(pos, _pet.transform.position);

        var isPlayerCloseEnough = distanceToPlayer <= distanceFromPlayerToTrigger;
        var isPetCloseEnough = distanceToPet <= distanceFromPetToTrigger;
        if (_hasHelpText) _helpTextRenderer.enabled = isPlayerCloseEnough && isEnabled;
        if (isPlayerCloseEnough)
        {
            if (!_isPlayerInRange)
            {
                onPlayerEnter?.Invoke();
                _isPlayerInRange = true;
            }

            if (isActivationKeyPressed && isEnabled)
            {
                onPlayerInteract?.Invoke();
                isEnabled = false;
            }
            else if (_hasHelpText)
            {
                Color textColor = _helpTextRenderer.material.color;
                textColor.a =
                    Mathf.Clamp(0.2f + Mathf.Abs(Mathf.Sin(2f * Mathf.PI * helpTextFlashingFrequency * Time.time)), 0f,
                        1f);
                _helpTextRenderer.material.color = textColor;
            }
        }
        else
        {
            if (reEnableAfterLeave)
            {
                isEnabled = true;
            }

            if (_isPlayerInRange)
            {
                onPlayerLeave?.Invoke();
                _isPlayerInRange = false;
            }
        }

        if (isPetCloseEnough)
        {
            if (!_isPetInRange)
            {
                onPetEnter?.Invoke();
                _isPetInRange = true;
            }
        }
        else if (_isPetInRange)
        {
            onPetLeave?.Invoke();
            _isPetInRange = false;
        }
    }
}