using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Events;
using OpenAI;
using OpenAI.Chat;
using UnityEngine;

public class HistoryManager : SingleTon<HistoryManager>,
    EventListener<OnRequestConversation>
{
    [Header(" Elements ")] 
    [SerializeField] private Transform historyMessagesParent;
    [SerializeField] private HistoryMessageContainerUI _historyMessageContainerUI;

    [Header("Data")] 
    private History _history;

    private string dataPath => Application.dataPath + "/History.json";

    private void Awake()
    {
        LoadData().Forget();
    }

    private async UniTask Init()
    {
        while (historyMessagesParent.childCount > 0)
        {
            Transform firstChild = historyMessagesParent.GetChild(0);
            firstChild.SetParent((null));
            Destroy(firstChild.gameObject);
        }

        for (int i = 0; i < _history.conversations.Count; i++)
        {
            HistoryMessageContainerUI messageContainerUI =
                Instantiate(_historyMessageContainerUI, historyMessagesParent);

            messageContainerUI.Configure(_history.conversations[i]);
        }
    }
    
    public void OnEvent(OnRequestConversation eventType)
    {
        _history.conversations.Remove(eventType.conversation);
    }
    
    public void SaveConverSation()
    {
        Message[] messages = DiscussionManager.Instance.GetMessages();
        
        // History => List of Conversation List of Mesasge;
        if(messages.Length < 3)
            return;

        Histories.Message[] myMessages = new Histories.Message[messages.Length];
        
        for (int i = 0; i < myMessages.Length; i++)
        {
            bool isUser = messages[i].Role == Role.User ? true : false;
            myMessages[i] = new Histories.Message(isUser,messages[i].Content.ToString());
        }

        Conversation conversation = new Conversation();
        conversation.date = System.DateTime.Now.Date.ToShortDateString();
        conversation.messages = myMessages;

        _history.conversations.Add(conversation);

        SaveData();

        Init();

        //JsonUtility.ToJson()
    }
    
    private void SaveData()
    {
        string data = JsonUtility.ToJson(_history,true);
        
        File.WriteAllText(dataPath,data);
    }
    
    private async UniTask LoadData()
    {
        string data = string.Empty;

        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            fs.Close();

            _history = new History();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            _history = JsonUtility.FromJson<History>(data);

            if (_history == null)
                _history = new History();
        }

        await Init();
    }

    private void OnEnable()
    {
        this.EventStartingListening<OnRequestConversation>();
    }
    
    private void OnDisable()
    {
        this.EventStopListening<OnRequestConversation>();
    }
}

[Serializable]
public class History
{
    public List<Conversation> conversations;

    public History()
    {
        conversations = new List<Conversation>();
    }
}

[Serializable]
public class Conversation
{
    public string date;
    public Histories.Message[] messages;
}

namespace Histories
{
    [Serializable]
    public class Message
    {
        public bool isUser;
        public string content;

        public Message(bool isUser, string content)
        {
            this.isUser = isUser;
            this.content = content;
        }
    }
}
