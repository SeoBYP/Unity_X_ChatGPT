using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using OpenAI;
using OpenAI.Chat;
using Rolos;
using UnityEngine.Serialization;


public class DiscussionManager : MonoBehaviour
{
    public static DiscussionManager Instance;
    
    [Header(" Elements ")]
    [SerializeField] private DiscussionBubble bubblePrefab;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform bubblesParent;

    [Header(" Events ")]
    public static Action onMessageReceived;
    public static Action<string> onChatGPTMessageReceived;
    public static Action onMessageSent;
    public static Action onNoMessageSent;
    [Header(" Authentication ")]
    [SerializeField] private string[] apiKey;
    [SerializeField] private string[] organizationId;
    private OpenAIClient api;

    [FormerlySerializedAs("Messages")]
    [Header(" Settings ")]
    [SerializeField] private List<Message> chatPrompts = new List<Message>();

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
        
        RoleContainerUI.onButtonClicked += RoleButtonClickedCallBack;
        HistoryMessageContainerUI.onHistoryButtonClicked += LoadHistory;
    }
    private void OnDisable()
    {
        RoleContainerUI.onButtonClicked -= RoleButtonClickedCallBack;
        HistoryMessageContainerUI.onHistoryButtonClicked -= LoadHistory;
    }



    // Start is called before the first frame update
    void Start()
    {
        //CreateBubble("Hey There ! How can I help you ?", false);
        
        Authenticate();
        
        //Initiliaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RoleButtonClickedCallBack(Roles roles)
    {
        chatPrompts.Clear();
        ClearAllBubble();
        Message prompt = new Message(Role.System, roles.firstPrompt);
        chatPrompts.Add(prompt);  
    }
    
    private void Authenticate()
    {
        api = new OpenAIClient(new OpenAIAuthentication(apiKey[0], organizationId[0]));
    }

    private void Initiliaze()
    {
        Message prompt = new Message(Role.System, "You are a the best comedian in the world.");
        chatPrompts.Add(prompt);  
    }

    private void LoadHistory(Conversation conversation)
    {
        chatPrompts.Clear();
        ClearAllBubble();

        for (int i = 0; i < conversation.messages.Length; i++)
        {
            Histories.Message msg = conversation.messages[i];
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
        if (!ActivityManager.Instance.CanSendMessage())
        {
            onNoMessageSent?.Invoke();
            return;
        }
        
        if (inputField.text.Length <= 0)
            return;
        
        CreateBubble(inputField.text, true);
        //CreateBubble("Message received : " + Random.Range(0, 100), false);

        Message prompt = new Message(Role.User, inputField.text);
        chatPrompts.Add(prompt);

        inputField.text = "";
        onMessageSent?.Invoke();
        ChatRequest request = new ChatRequest(
            messages: chatPrompts,
            model: OpenAI.Models.Model.GPT3_5_Turbo,
            temperature: 0.2);

        try
        {
            var result = await api.ChatEndpoint.GetCompletionAsync(request);

            Message chatResult = new Message(Role.System, result.FirstChoice.ToString());
            chatPrompts.Add(chatResult);

            onChatGPTMessageReceived?.Invoke(result.FirstChoice.ToString());

            CreateBubble(result.FirstChoice.ToString(), false);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

    private void CreateBubble(string message, bool isUserMessage)
    {
        DiscussionBubble discussionBubble = Instantiate(bubblePrefab, bubblesParent);
        discussionBubble.Configure(message, isUserMessage);

        onMessageReceived?.Invoke();
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
}
