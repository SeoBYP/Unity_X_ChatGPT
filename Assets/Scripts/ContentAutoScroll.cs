using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using UnityEditor.Rendering;
using UnityEngine;

public class ContentAutoScroll : MonoBehaviour
{
    private RectTransform contentRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        contentRectTransform = GetComponent<RectTransform>();
    }
    
// 자식 오브젝트들이 추가/변경될 때 호출
    public async UniTask UpdateContentSizeAndScroll()
    {
        // 변경 사항이 적용될 시간을 기다립니다.
        await UniTask.DelayFrame(1); // 한 프레임 대기
        
        if(contentRectTransform == null)
            contentRectTransform = GetComponent<RectTransform>();
        
        // 모든 자식 오브젝트들의 높이 합계 계산
        float totalHeight = 0;
        foreach (RectTransform child in contentRectTransform)
        {
            totalHeight += child.sizeDelta.y; // 각 자식의 높이와 간격 추가
        }

        // Content의 sizeDelta 업데이트
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);

        // 비동기로 스크롤 다운 호출
        await ScrollDown();
    }

    // 스크롤을 가장 아래로 내리기
    public async UniTask ScrollDown()
    {
        // 변경 사항이 적용될 시간을 기다립니다.
        await UniTask.DelayFrame(1); // 한 프레임 대기

        // 스크롤을 아래로 내립니다.
        contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, 0);
    }
}
