using System;
using System.Collections;
using System.Collections.Generic;
using Rolos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryMessageContainerUI : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI firstPromptText;
    [SerializeField] private Button _button;
    
    [Header("Data")] private Conversation _conversation;

    [Header("Actions")] public static Action<Conversation> onHistoryButtonClicked;
    
    public void Configure(Conversation conversation)
    {
        _conversation = conversation;
        _button.onClick.AddListener(ButtonClickedCallback);
        
        string date = conversation.date;
        string firstPrompt = conversation.messages[0].content;

        dateText.text = date;
        firstPromptText.text = firstPrompt;
        //CreateRoleContainers(roleCategoryData);
    }

    public void ButtonClickedCallback()
    {
        onHistoryButtonClicked?.Invoke(_conversation);
    }
}
