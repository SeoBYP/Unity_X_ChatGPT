using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentAutoScroll : MonoBehaviour
{
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();

        DiscussionManager.onMessageReceived += DelayScrollDown;
    }

    private void OnDestroy()
    {
        DiscussionManager.onMessageReceived -= DelayScrollDown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DelayScrollDown()
    {
        Invoke("ScrollDown", .3f);
    }

    private void ScrollDown()
    {
        Vector2 anchoredPosition = rt.anchoredPosition;
        anchoredPosition.y = Mathf.Max(0, rt.sizeDelta.y);
        rt.anchoredPosition = anchoredPosition;
    }
}
