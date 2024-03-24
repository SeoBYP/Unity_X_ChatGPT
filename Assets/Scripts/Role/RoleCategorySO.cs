using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;


namespace Rolos
{
    [CreateAssetMenu(fileName = "Roles Category",menuName = "Roles/Roles Category")]
    public class RoleCategorySO : ScriptableObject
    {
        public string title;
        public Roles[] roles;
    }
}
