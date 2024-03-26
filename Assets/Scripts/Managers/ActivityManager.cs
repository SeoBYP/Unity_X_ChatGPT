using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager : SingleTon<ActivityManager>,
    EventListener<OnMessageSent>
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI messageSentText;
    [SerializeField] private TextMeshProUGUI messageLeftText;
    [SerializeField] private ProgressBar progressBar;
    
    [Header("Data")] 
    [SerializeField] private int freeMessagesCount = 300;
    
    private int messagesSent;
    private int messagesLeft;
    private const string messagesSentKey = "messagesSent";
    private const string messagesLeftKey = "messagesLeft";
    
    private void Start()
    {
        LoadData();
        UpdateUI();
    }
    public void OnEvent(OnMessageSent eventType)
    {
        messagesSent++;
        messagesLeft--;
        
        UpdateUI();
        SaveData();
    }

    private void ResetMessages()
    {
        messagesLeft = freeMessagesCount;
        
        LoadData();
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        float usagePercent = (float)((freeMessagesCount - messagesLeft) / freeMessagesCount);
        progressBar.currentPercent = usagePercent;
        messageSentText.text = messagesSent + " Messages Sent";
        messageLeftText.text = messagesLeft + " Messages Left";
    }

    private void LoadData()
    {
        messagesSent = PlayerPrefs.GetInt(messagesSentKey, 0);
        messagesLeft = PlayerPrefs.GetInt(messagesLeftKey, 300);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(messagesSentKey,messagesSent);
        PlayerPrefs.SetInt(messagesLeftKey, messagesLeft);
    }

    public bool CanSendMessage()
    {
        return messagesLeft > 0;
    }

    private void OnEnable()
    {
        this.EventStartingListening<OnMessageSent>();
    }

    private void OnDisable()
    {
        this.EventStopListening<OnMessageSent>();
    }
}
