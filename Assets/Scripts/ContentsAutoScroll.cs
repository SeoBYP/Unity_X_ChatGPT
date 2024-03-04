using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ContentsAutoScroll : MonoBehaviour
{
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ChatManager.onMessageReceived += ScrollDown;
    }

    void OnDestroy()
    {
        ChatManager.onMessageReceived -= ScrollDown;
    }

    private async void ScrollDown(){
        await UniTask.Delay(30);

        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.y = Mathf.Max(0,rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = anchoredPosition;
    }
}
