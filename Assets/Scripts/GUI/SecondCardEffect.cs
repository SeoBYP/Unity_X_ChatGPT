using UnityEngine;

public class SecondCardEffect : MonoBehaviour
{
    private StoryCardEffect _storyCardEffect; // 첫 번째 카드에 적용된 StoryCardEffect 컴포넌트를 참조합니다.
    private GameObject _firstCard; // 첫 번째 카드의 GameObject를 참조합니다.
    
    void Start()
    {
        _storyCardEffect = FindObjectOfType<StoryCardEffect>(); // Scene 내에서 StoryCardEffect 컴포넌트를 찾아 참조합니다.
        _firstCard = _storyCardEffect.gameObject; // 찾은 StoryCardEffect 컴포넌트가 부착된 GameObject(첫 번째 카드)를 참조합니다.
        _storyCardEffect.cardMoved += StoryCardMovedFront; // 첫 번째 카드가 움직였을 때 호출될 메서드를 이벤트에 등록합니다.
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // 두 번째 카드의 초기 크기를 설정합니다.
    }
    
    void Update()
    {
        float distanceMoved = _firstCard.transform.localPosition.x; // 첫 번째 카드가 이동한 거리를 계산합니다.
        if (Mathf.Abs(distanceMoved) > 0)
        {
            // 첫 번째 카드가 이동함에 따라 두 번째 카드의 크기를 점진적으로 변경합니다.
            float step = Mathf.SmoothStep(0.8f, 1, Mathf.Abs(distanceMoved) / (Screen.width / 2));
            transform.localScale = new Vector3(step, step, step); // 두 번째 카드의 크기를 업데이트합니다.
        }
    }

    void StoryCardMovedFront()
    {
        gameObject.AddComponent<StoryCardEffect>(); // 첫 번째 카드가 움직임을 마친 후 두 번째 카드에 StoryCardEffect 컴포넌트를 추가합니다.
        Destroy(this); // SecondCard 컴포넌트 자체를 제거합니다. 이제 이 객체는 StoryCardEffect의 동작을 따릅니다.
    }
}

