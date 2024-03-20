using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCard : MonoBehaviour
{
    private StoryCardEffect _storyCardEffect;
    private GameObject _firstCard;
    
    // Start is called before the first frame update
    void Start()
    {
        _storyCardEffect = FindObjectOfType<StoryCardEffect>();
        _firstCard = _storyCardEffect.gameObject;
        _storyCardEffect.cardMoved += StoryCardMovedFront;
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = _firstCard.transform.localPosition.x;
        if (Mathf.Abs(distanceMoved) > 0)
        {
            float step = Mathf.SmoothStep(0.8f, 1, Mathf.Abs(distanceMoved) / (Screen.width / 2));
            transform.localScale = new Vector3(step, step, step);
        }
    }

    void StoryCardMovedFront()
    {
        gameObject.AddComponent<StoryCardEffect>();
        Destroy(this);
    }
}
