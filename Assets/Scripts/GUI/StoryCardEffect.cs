using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoryCardEffect : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 _initialPosition; // 카드의 초기 위치입니다.
    private float _distanceMoved; // 초기 위치에서 카드가 이동한 거리입니다.
    private bool _swipeLeft; // 스와이프 방향을 결정하는 플래그입니다. 왼쪽으로 스와이프했는지 여부입니다.

    public event Action cardMoved; // 카드가 상당히 이동했을 때 트리거되는 이벤트입니다.

    // 드래그가 시작될 때 호출됩니다.
    public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = transform.localPosition; // 카드의 초기 위치를 저장합니다.
    }

    // 드래그하는 동안 계속해서 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 델타를 기반으로 카드를 이동시킵니다.
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);

        // 드래그 방향과 거리에 기반하여 카드를 회전시킵니다.
        if (transform.localPosition.x - _initialPosition.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, -30,
                (_initialPosition.x + transform.localPosition.x) / (Screen.width / 2)));
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 30,
                (_initialPosition.x - transform.localPosition.x) / (Screen.width / 2)));
        }
    }

    // 드래그가 끝났을 때 호출됩니다.
    public void OnEndDrag(PointerEventData eventData)
    {
        _distanceMoved = Mathf.Abs(transform.localPosition.x - _initialPosition.x); // 이동한 거리를 계산합니다.
        if (_distanceMoved < 0.4 * Screen.width)
        {
            // 카드가 충분히 이동하지 않았다면, 원래의 위치와 방향으로 되돌립니다.
            transform.localPosition = _initialPosition;
            transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            // 애니메이션을 위한 스와이프 방향을 결정합니다.
            _swipeLeft = transform.localPosition.x <= _initialPosition.x;
            cardMoved?.Invoke(); // 카드 이동 이벤트를 호출합니다.
            StartCoroutine(MovedCard()); // 카드 제거 애니메이션을 시작합니다.
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0;
        // 카드가 화면 밖으로 이동하고 페이드아웃되면서 사라지는 애니메이션입니다.
        while (GetComponent<Image>().color != new Color(1, 1, 1, 0))
        {
            time += Time.deltaTime;
            if (_swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x - Screen.width, time), transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + Screen.width, time), transform.localPosition.y, 0);
            }

            // 카드의 색상을 점차적으로 투명하게 변경하여 사라지게 합니다.
            GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null; // 다음 프레임까지 기다립니다.
        }

        Destroy(gameObject); // 카드 게임 오브젝트를 파괴합니다.
    }
}