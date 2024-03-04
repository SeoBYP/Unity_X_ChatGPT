using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image BubbleImage;
    [SerializeField] private Sprite userBubbleSprite;

    [Header("Settings")]
    [SerializeField] private Color userBubbleColor;

    public void Configure(string message, bool isUserMessage)
    {
        if (isUserMessage)
        {
            BubbleImage.sprite = userBubbleSprite;
            BubbleImage.color = userBubbleColor;
            messageText.color = Color.white;
        }

        messageText.text = message;
        messageText.ForceMeshUpdate();
    }
}
