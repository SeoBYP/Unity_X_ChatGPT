using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Events;
using OpenAI;
using OpenAI.Chat;
using Rolos;
using UnityEngine.Serialization;


public class DiscussionManager : SingleTon<DiscussionManager>,
    EventListener<OnRequestRole>,
    EventListener<OnRequestConversation>
{
    [Header(" Elements ")]
    [SerializeField] private DiscussionBubble bubblePrefab;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform bubblesParent;
    [SerializeField] private GameObject noMoreAIModel;
    [SerializeField] private ContentAutoScroll contentAutoScroll;
    [Header(" Authentication ")]
    [SerializeField] private string[] apiKey;
    [SerializeField] private string[] organizationId;
    private OpenAIClient api;

    [FormerlySerializedAs("Messages")]
    [Header(" Settings ")]
    [SerializeField] private List<Message> chatPrompts = new List<Message>();
    
    public bool HasRoles => currentRoles != null;
    private Roles currentRoles;
        
    // Start is called before the first frame update
    void Start()
    {
        Authenticate();
    }

    private void Authenticate()
    {
        api = new OpenAIClient(new OpenAIAuthentication(apiKey[0], organizationId[0]));
    }

    public void ClearAIModel()
    {
        currentRoles = null;
        noMoreAIModel.SetActive(true);
        chatPrompts.Clear();
        inputField.text = "";
        ClearAllBubble();
    }

    public void Initialize()
    {
        if (HasRoles)
        {
            noMoreAIModel.SetActive(false);
        }
        else
        {
            noMoreAIModel.SetActive(true);
        }
    }
    
    public void OnEvent(OnRequestRole eventType)
    {
        currentRoles = eventType.roles;
        chatPrompts.Clear();
        ClearAllBubble();
        Message prompt = new Message(Role.System, eventType.roles.firstPrompt);
        chatPrompts.Add(prompt);  
        CreateBubble(eventType.roles.firstChat, false);
        Initialize();
    }

    public void OnEvent(OnRequestConversation eventType)
    {
        chatPrompts.Clear();
        ClearAllBubble();

        for (int i = 0; i < eventType.conversation.messages.Length; i++)
        {
            Histories.Message msg = eventType.conversation.messages[i];
            Role role;
            if (i == 0)
                role = Role.System;
            else
                role = msg.isUser ? Role.User : Role.System;

            chatPrompts.Add(new Message(role, msg.content));
            CreateBubble(msg.content,msg.isUser);
        }
    }
    
    public async void AskButtonCallback()
    {
        if (!HasRoles)
        {
            return;
        }
        if (!ActivityManager.Instance.CanSendMessage())
        {
            OnNoMoreChat.Trigger();
            return;
        }
        
        if (inputField.text.Length <= 0)
            return;
        
        CreateBubble(inputField.text, true);
        //CreateBubble("Message received : " + Random.Range(0, 100), false);

        Message prompt = new Message(Role.User, inputField.text);
        chatPrompts.Add(prompt);

        inputField.text = "";
        OnMessageSent.Trigger();
        ChatRequest request = new ChatRequest(
            messages: chatPrompts,
            model: OpenAI.Models.Model.GPT3_5_Turbo,
            temperature: 0.2);

        try
        {
            var result = await api.ChatEndpoint.GetCompletionAsync(request);

            Message chatResult = new Message(Role.System, result.FirstChoice.ToString());
            chatPrompts.Add(chatResult);

            OnChatGPTMessageReceived.Trigger(result.FirstChoice.ToString());

            CreateBubble(result.FirstChoice.ToString(), false);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

    private async void CreateBubble(string message, bool isUserMessage)
    {
        DiscussionBubble discussionBubble = Instantiate(bubblePrefab, bubblesParent);
        discussionBubble.Configure(message, isUserMessage);

        await contentAutoScroll.UpdateContentSizeAndScroll();
    }

    private void ClearAllBubble()
    {
        while (bubblesParent.childCount > 0)
        {
            Transform firstChild = bubblesParent.GetChild(0);
            firstChild.SetParent((null));
            Destroy(firstChild.gameObject);
        }
    }

    public Message[] GetMessages()
    {
        return chatPrompts.ToArray();
    }
    
    private void OnEnable()
    {
        this.EventStartingListening<OnRequestRole>();
        this.EventStartingListening<OnRequestConversation>();
    }
    private void OnDisable()
    {
        this.EventStopListening<OnRequestRole>();
        this.EventStopListening<OnRequestConversation>();
    }
}