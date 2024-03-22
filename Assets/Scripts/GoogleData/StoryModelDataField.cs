using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    public class StoryModelDataField : ScriptableObject
    {
        [BoxGroup("스토리 정보")] [SerializeField, TextArea]
        public string StoryResourceID = "Resource Name";

        [BoxGroup("스토리 정보")] [SerializeField, TextArea]
        public string StoryTitle = "Story Title";

        [BoxGroup("스토리 정보")] [SerializeField, MultiLineProperty(20)]
        public string StoryDescription = "Story Description";
    }
}