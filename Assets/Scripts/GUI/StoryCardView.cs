using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class StoryCardView : MonoBehaviour
{
    public GameObject storyCardPrefab;
    
    private void Update()
    {
        if (transform.childCount < 2)
        {
            InstantiateNewStoryCard();
        }
    }
    
    private void InstantiateNewStoryCard()
    {
        GameObject newCard = Instantiate(storyCardPrefab, transform, false);
        newCard.GetOrAddComponent<SecondCard>();
        newCard.transform.SetAsFirstSibling();

        StoryCard storyCard = newCard.gameObject.GetComponent<StoryCard>();
        storyCard.Init();
    }
}
