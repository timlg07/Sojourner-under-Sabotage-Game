using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance => _instance;
    private static DialogueUI _instance;
    
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI _dialogueText;
    [SerializeField] private KeyCode _activationKey = KeyCode.Return;
    
    private List<string> _currentDialogue;
    private int _currentDialogueIndex;
    private bool _isActivationKeyPressed;
    private DateTime _lastActivationKeyPressedTime = DateTime.Now;
    private bool _hasDialogueToShow = false;

    private void RegisterInstance(DialogueUI dialogueUI)
    {
        if (_instance == null)
        {
            _instance = dialogueUI;
        }
        else
        {
            Debug.LogError("There is already a DialogueUI instance in the scene!");
        }
    }
    
    void Awake()
    {
        RegisterInstance(this);
    }

    private void Start()
    {
        _dialoguePanel.SetActive(false);
    }

    void Update()
    {
        var timeSinceLastActivationKeyPressed = DateTime.Now - _lastActivationKeyPressedTime;
        var secondsSinceLastActivationKeyPressed = timeSinceLastActivationKeyPressed.TotalSeconds;
        _isActivationKeyPressed = _isActivationKeyPressed && Input.GetKey(_activationKey) 
                                  || Input.GetKeyDown(_activationKey) && secondsSinceLastActivationKeyPressed > 0.25f;
        
        if (!_hasDialogueToShow) return;
        
        if (_isActivationKeyPressed)
        {
            if (_currentDialogueIndex < _currentDialogue.Count - 1)
            {
                _currentDialogueIndex++;
                _dialogueText.text = _currentDialogue[_currentDialogueIndex];
                ResetActivationKey();
            }
            else
            {
                _currentDialogueIndex = -1;
                _dialogueText.text = "";
                _hasDialogueToShow = false;
                _dialoguePanel.SetActive(false);
                StompEventDelegation.OnConversationFinished();
            }
        }
    }

    public void ShowDialogue(List<string> dialogue)
    {
        if (_currentDialogue == dialogue)
        {
            Debug.LogWarning("Dialogue is already being shown!");
            return;
        }
        
        _currentDialogueIndex = 0;
        _currentDialogue = dialogue;
        _hasDialogueToShow = true;
        
        // TODO: DOTween to fade in dialogue panel
        _dialoguePanel.SetActive(true);
        
        _dialogueText.text = _currentDialogue[_currentDialogueIndex];
        
        ResetActivationKey();
    }
    
    private void ResetActivationKey()
    {
        _isActivationKeyPressed = false;
        _lastActivationKeyPressedTime = DateTime.Now;
    }
}