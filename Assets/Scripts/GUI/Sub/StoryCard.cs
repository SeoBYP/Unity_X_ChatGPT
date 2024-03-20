using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
public class StoryCardData
{
    public ReactiveProperty<string> Title { get; } = new();

    public ReactiveProperty<Sprite> Icon { get; } = new();

    public StoryCardData(string title, Sprite icon)
    {
        Title.Value = title;
        Icon.Value = icon;
    }

}
