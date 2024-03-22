using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UniRx;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class StoryCardView : MonoBehaviour
{
    public GameObject storyCardPrefab;
    public List<StoryModelDataField> storyModelDataTable;

    public IntReactiveProperty Index = new();

    private void Start()
    {
        Index.Value = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            StoryCard storyCard = transform.GetChild(i).GetComponent<StoryCard>();
            storyCard.Init(storyModelDataTable[Index.Value]);
            Index.Value++;
        }
    }

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
        newCard.GetOrAddComponent<SecondCardEffect>();
        newCard.transform.SetAsFirstSibling();

        StoryCard storyCard = newCard.GetComponent<StoryCard>();
        storyCard.Init(storyModelDataTable[Index.Value]);
        Index.Value++;
        if (Index.Value >= storyModelDataTable.Count)
        {
            Index.Value = 0;
        }
    }
}
