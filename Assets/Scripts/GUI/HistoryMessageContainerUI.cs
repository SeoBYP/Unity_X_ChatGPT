using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Michsky.MUIP;
using Rolos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryMessageContainerUI : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI firstPromptText;
    [SerializeField] private ButtonManager _button;
    
    [Header("Data")] private Conversation _conversation;
    
    public void Configure(Conversation conversation)
    {
        _conversation = conversation;
        _button.onClick.AddListener(ButtonClickedCallback);

        int lastIndex = conversation.messages.Length - 1;
        string date = conversation.date;
        string firstPrompt = conversation.messages[lastIndex].content;

        dateText.text = date;
        firstPromptText.text = firstPrompt;
        //CreateRoleContainers(roleCategoryData);
    }

    public void ButtonClickedCallback()
    {
        OnRequestConversation.Trigger(_conversation);
    }
}
