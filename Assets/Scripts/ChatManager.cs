using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ChatBubble chatBubble;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform bubbleParent;

    [Header("Events")]
    public static Action onMessageReceived;

    [Header("Settings")]
    [SerializeField] private List<Message> chatPrompts = new List<Message>();

    // Start is called before the first frame update
    async void Start()
    {
        Initialize().Forget();
        await UniTask.Delay(200);
        CreateBubble("안녕하세요 무엇이든 물어보세요!", false);
    }

    public async UniTaskVoid Initialize()
    {
        Message prompt = new Message(Role.System, "You are a helpful assistant.");
        chatPrompts.Add(prompt);
    }

    public async void AskButtonCallback()
    {
        CreateBubble(inputField.text, true);

        Message prompt = new Message(Role.User, inputField.text);
        chatPrompts.Add(prompt);
        inputField.text = "";
        try
        {
            var result = await NetworkSystem.Instance.GetChatResponse(chatPrompts);


            Message chatResult = new Message(Role.System, result);
            chatPrompts.Add(chatResult);

            CreateBubble(result, false);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    private void CreateBubble(string message, bool isUserMessage)
    {
        ChatBubble discussionBubble = Instantiate(chatBubble, bubbleParent);
        chatBubble.Configure(message, isUserMessage);
        onMessageReceived?.Invoke();
    }
}
