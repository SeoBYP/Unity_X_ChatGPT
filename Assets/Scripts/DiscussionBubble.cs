using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Events;

public class DiscussionBubble : MonoBehaviour
{
    [Header(" Elements ")] 
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image bubbleImage;
    [SerializeField] private Sprite userBubbleSprite;
    [SerializeField] private GameObject voiceButton;

    [Header(" Settings ")] 
    [SerializeField] private Color userBubbleColor;
    [SerializeField] private float duration = 2.0f;
    
    [Header(" Events ")] public static Action<string> onVoiceButtonClicked;
    
    public async UniTask Configure(string message, bool isUserMessage)
    {
        if (isUserMessage)
        {
            bubbleImage.sprite = userBubbleSprite;
            bubbleImage.color = userBubbleColor;
            messageText.color = Color.white;

            voiceButton.SetActive(false);
        }

        await AnimateTextAsync(message, duration);
        messageText.ForceMeshUpdate();
    }

    private async UniTask AnimateTextAsync(string message, float totalDuration)
    {
        messageText.text = ""; // 메시지 텍스트 초기화

        if (message.Length == 0) return; // 메시지가 비어있으면 바로 반환

        float delay = totalDuration / message.Length; // 각 문자에 대한 딜레이 시간 계산

        for (int i = 0; i < message.Length; i++)
        {
            messageText.text += message[i]; // 문자 하나씩 추가
            await UniTask.Delay(TimeSpan.FromSeconds(delay)); // 설정된 딜레이만큼 대기
            OnUpdateScroll.Trigger();
        }
    }

    public void VoiceButtonCallback()
    {
        onVoiceButtonClicked?.Invoke(messageText.text);
    }

    public void CopyToClipboardCallback()
    {
        GUIUtility.systemCopyBuffer = messageText.text;
    }
}