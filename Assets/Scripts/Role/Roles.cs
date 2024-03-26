namespace Rolos
{
    public class Roles
    {
        public string title;
        public string firstPrompt;
        public string firstChat;
        public Roles(RoleCategorySO roleCategoryData)
        {
            title = roleCategoryData.title;
            firstPrompt = roleCategoryData.firstPrompt;
            firstChat = roleCategoryData.firstBubble;
        }
    }
}