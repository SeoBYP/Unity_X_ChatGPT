using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using UnityEditor.Rendering;
using UnityEngine;

public class ContentAutoScroll : MonoBehaviour,
    EventListener<OnUpdateScroll>
{
    private RectTransform contentRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        contentRectTransform = GetComponent<RectTransform>();
    }
    public void OnEvent(OnUpdateScroll eventType)
    {
        ScrollDown().Forget();
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

        // ScrollView의 RectTransform을 가져옵니다. 이는 contentRectTransform의 부모입니다.
        RectTransform scrollViewRectTransform = contentRectTransform.parent.GetComponent<RectTransform>();

        // 콘텐츠의 높이가 스크롤 뷰의 높이보다 큰 경우만 스크롤을 조정합니다.
        if (contentRectTransform.sizeDelta.y > scrollViewRectTransform.sizeDelta.y)
        {
            // 스크롤을 가장 아래로 내리기 위해, anchoredPosition.y를 콘텐츠의 높이와 스크롤 뷰의 높이의 차이로 설정합니다.
            float newY = contentRectTransform.sizeDelta.y - scrollViewRectTransform.sizeDelta.y;
            contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, newY);
        }
        else
        {
            // 콘텐츠의 높이가 스크롤 뷰의 높이보다 작거나 같은 경우, 스크롤을 조정할 필요가 없습니다.
            contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, 0);
        }
    }

    private void OnEnable()
    {
        this.EventStartingListening<OnUpdateScroll>();
    }

    private void OnDisable()
    {
        this.EventStopListening<OnUpdateScroll>();
    }
}
