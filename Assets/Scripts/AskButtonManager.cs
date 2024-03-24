using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AskButtonManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private DiscussionManager discussionManager;
    [SerializeField] private STTBridge sttBridge;
    [SerializeField] private TMP_InputField promptInputField;

    [Header(" Graphics ")]
    [SerializeField] private GameObject askText; 
    [SerializeField] private GameObject micImage; 

    [Header(" Settings ")]
    private bool recording;

    // Start is called before the first frame update
    void Start()
    {
        ShowMicImage();
        promptInputField.onValueChanged.AddListener(InputFieldValueChangedCallback);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointerDownCallback()
    {
        if(promptInputField.text.Length > 0)
        {
            discussionManager.AskButtonCallback();
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

        if(prompt.Length <= 0)
        {
            ShowMicImage();
        }
        else
        {
            ShowAskText();
        }
    }

    private void ShowMicImage()
    {
        micImage.SetActive(true);
        askText.SetActive(false);
    }

    private void ShowAskText()
    {
        askText.SetActive(true);
        micImage.SetActive(false);
    }
}
