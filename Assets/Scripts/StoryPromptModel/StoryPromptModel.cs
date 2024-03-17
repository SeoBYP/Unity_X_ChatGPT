using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity_X_ChatGPT.Assets.Scripts;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;
using UnityEngine.UIElements;

namespace StoryModel
{
    [CreateAssetMenu(fileName = "New StoryPromptModel",menuName = "StoryModel/New StoryPromptModel")]
    public class StoryPromptModel : SerializedScriptableObject
    {
        [BoxGroup("스토리 정보")] 
        [SerializeField, TextArea] public string StoryTitle = "Story Title";
        
        [BoxGroup("스토리 정보")] 
        [SerializeField, MultiLineProperty(20)] public  string StoryDescription = "Story Description";
    }
}