using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTResultImage : MonoBehaviour
{
    [SerializeField] private Image Icon;

    void Start()
    {
        Icon.enabled = false;
    }
    public void Configure(Texture2D texture2D)
    {
        Icon.enabled = true;
        // Texture2D의 실제 픽셀 크기를 사용하여 Rect 생성
        Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
        // 스프라이트의 중심을 피벗으로 설정
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite icon = Sprite.Create(texture2D, rect, pivot);
        Icon.sprite = icon;
        Icon.SetNativeSize();
    }

}
