using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
{
    public static ActivityManager Instance;
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI messageSentText;
    [SerializeField] private TextMeshProUGUI messageLeftText;
    [SerializeField] private TextMeshProUGUI usageText;
    [SerializeField] private Image usageFillImage;
    
    [Header("Data")] 
    [SerializeField] private int freeMessagesCount = 300;
    private int messagesSent;
    private int messagesLeft;
    private const string messagesSentKey = "messagesSent";
    private const string messagesLeftKey = "messagesLeft";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        DiscussionManager.onMessageSent += MessageSentCallback;
    }

    private void OnDisable()
    {
        DiscussionManager.onMessageSent -= MessageSentCallback;
    }
    
    private void Start()
    {
        LoadData();
        UpdateUI();
    }
    
    private void MessageSentCallback()
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
        usageFillImage.fillAmount = usagePercent;
        usageText.text = (usagePercent * 100).ToString() + "%";
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
}
