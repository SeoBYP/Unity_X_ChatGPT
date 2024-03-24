using System;
using System.Collections;
using System.Collections.Generic;
using Rolos;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private CanvasGroup chatCG;
    [SerializeField] private CanvasGroup mainCG;
    [SerializeField] private CanvasGroup popupCG;

    [Header(" Events ")] 
    public static Action onBackPromptChat;
    
    private void OnEnable()
    {
        RoleContainerUI.onButtonClicked += RoleButtonClicked;
        DiscussionManager.onNoMessageSent += ShowNoMessages;
        HistoryMessageContainerUI.onHistoryButtonClicked += ShowChat;
    }



    private void OnDisable()
    {
        RoleContainerUI.onButtonClicked -= RoleButtonClicked;
        DiscussionManager.onNoMessageSent -= ShowNoMessages;
        HistoryMessageContainerUI.onHistoryButtonClicked -= ShowChat;
    }

    private void Start()
    {
        HideChat();
        HideNoMessages();
    }

    private void RoleButtonClicked(Roles roles)
    {
        ShowChat();
    }

    public void BackToMenuFromChat()
    {
        HideChat();
        onBackPromptChat?.Invoke();
    }

    private void ShowChat()
    {
        ShowCanvasGroup(chatCG);
    }
    
    private void ShowChat(Conversation obj)
    {
        ShowCanvasGroup(chatCG);
    }
    
    private void HideChat()
    {
        HideCanvasGroup(chatCG);
    }
    
    public void ShowNoMessages()
    {
        ShowCanvasGroup(popupCG);
    }
    
    public void HideNoMessages()
    {
        HideCanvasGroup(popupCG);
    }
    public void ShowCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    
    public void HideCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
