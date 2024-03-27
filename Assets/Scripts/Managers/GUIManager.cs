using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using Michsky.MUIP;
using Rolos;
using UnityEngine;
using UnityEngine.UI;

public enum EWindows
{
    UserActivity,
    RoleSelect,
    Chatting
}
public class GUIManager : SingleTon<GUIManager>,
    EventListener<OnRequestRole>,
    EventListener<OnRequestConversation>,
    EventListener<OnNoMoreChat>
{
    [Header("Elements")] 
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private ModalWindowManager noMoreMessage;
    [SerializeField] private CanvasGroup spinnerPopup;
    [SerializeField] private UIManagerProgressBarLoop progressBarLoop;
    private void Start()
    {
        noMoreMessage.Close();
        HideSpinner().Forget();
        windowManager.onWindowChange.AddListener(ClearCurrentRole);
    }

    private void ClearCurrentRole(int index)
    {
        EWindows windows = (EWindows)index;
        switch (windows)
        {
            case EWindows.UserActivity:
            case EWindows.RoleSelect:
                HistoryManager.Instance.SaveConverSation();
                DiscussionManager.Instance.ClearAIModel();
                break;
            case EWindows.Chatting:
                DiscussionManager.Instance.Initialize();
                AskButtonManager.Instance.Initialize();
                break;
        }
    }
    
    public void OnEvent(OnRequestRole eventType)
    {
        ShowChat();
    }
    public void OnEvent(OnRequestConversation eventType)
    {
        ShowChat();
    }
    
    public void OnEvent(OnNoMoreChat eventType)
    {
        ShowNoMessages();
    }

    private void ShowChat()
    {
        windowManager.OpenWindowByIndex(2);
    }
    
    private void ShowChat(Conversation obj)
    {
        windowManager.OpenWindowByIndex(2);
    }
    
    public void ShowNoMessages()
    {
        noMoreMessage.Open();
        Button button = noMoreMessage.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(HideNoMessages);
    }
    
    public void HideNoMessages()
    {
        noMoreMessage.Close();
        windowManager.OpenWindowByIndex(0);
    }

    public async UniTask ShowSpinner()
    {
        spinnerPopup.blocksRaycasts = true;
        spinnerPopup.interactable = true;
        spinnerPopup.alpha = 1;
        progressBarLoop.gameObject.SetActive(true);
    }
    
    public async  UniTask HideSpinner()
    {
        spinnerPopup.blocksRaycasts = false;
        spinnerPopup.interactable = false;
        spinnerPopup.alpha = 0;
        progressBarLoop.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        this.EventStartingListening<OnRequestRole>();
        this.EventStartingListening<OnRequestConversation>();
        this.EventStartingListening<OnNoMoreChat>();
    }

    private void OnDisable()
    {
        this.EventStopListening<OnRequestRole>();
        this.EventStopListening<OnRequestConversation>();
        this.EventStopListening<OnNoMoreChat>();
    }
}
