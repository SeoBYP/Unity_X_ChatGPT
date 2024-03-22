using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Data
{
    public class StoryModelDataTable : SerializedScriptableObject
    {
        public Dictionary<int, StoryModelDataField> StoryModelDataFields;
    }
}