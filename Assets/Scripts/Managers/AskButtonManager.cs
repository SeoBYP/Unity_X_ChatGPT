using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

public class AskButtonManager : SingleTon<AskButtonManager>
{
    [Header(" Elements ")]
    [SerializeField] private STTBridge sttBridge;
    [SerializeField] private TMP_InputField promptInputField;

    [Header(" Graphics ")]
    [SerializeField] private ButtonManager askButton; 

    [Header(" Settings ")]
    private bool recording;

    // Start is called before the first frame update
    void Start()
    {
        askButton.onClick.AddListener(PointerDownCallback);
        askButton.Interactable(false);
        promptInputField.onValueChanged.AddListener(InputFieldValueChangedCallback);
    }

    public void Initialize()
    {
        askButton.Interactable(false);
    }

    public void PointerDownCallback()
    {
        if(promptInputField.text.Length > 0)
        {
            DiscussionManager.Instance.AskButtonCallback();
        }
        else
        {
            sttBridge.SetActivation(true);
            recording = true;
        }
    }

    public void PointerUpCallback()
    {
        sttBridge.SetActivation(false);
        recording = false;

        InputFieldValueChangedCallback(promptInputField.text);
    }

    private void InputFieldValueChangedCallback(string prompt)
    {
        if (recording)
            return;

        if(prompt.Length <= 0 
           || !DiscussionManager.Instance.HasRoles
           || DiscussionManager.Instance.CreatingBubble)
        {
            askButton.Interactable(false);
        }
        else
        {
            askButton.Interactable(true);
        }
    }
}
