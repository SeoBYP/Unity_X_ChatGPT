using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class StoryCard : MonoBehaviour
{
    [SerializeField] private Image StoryCardIcon;
    [SerializeField] private Image StoryCardFrame;
    [SerializeField] private TextMeshProUGUI StoryCardTitle;
    [SerializeField] private TextMeshProUGUI StoryCardDescription;

    public async void Init(StoryModelDataField field)
    {
        var task = await Addressables.LoadAssetAsync<Sprite>(field.StoryResourceID);
        StoryCardIcon.sprite = task;
        Addressables.Release(task);

        StoryCardTitle.text = field.StoryTitle;
        int index = field.StoryDescription.IndexOf("\n");
        string des = field.StoryDescription.Substring(0, index);
        StoryCardDescription.text = des;
    }
}
