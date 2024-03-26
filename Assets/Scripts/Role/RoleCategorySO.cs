using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;
using Sirenix.OdinInspector;


namespace Rolos
{
    [CreateAssetMenu(fileName = "Roles Category",menuName = "Roles/Roles Category")]
    public class RoleCategorySO : ScriptableObject
    {
        public string title;
        [MultiLineProperty(lines:15)]
        public string firstPrompt;
        [MultiLineProperty(lines:7)]
        public string des;
        [MultiLineProperty]
        public string firstBubble;
    }
}
