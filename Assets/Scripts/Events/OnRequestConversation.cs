namespace Events
{
    public struct OnRequestConversation
    {
        public Conversation conversation;

        public OnRequestConversation(Conversation conversation)
        {
            this.conversation = conversation;
        }

        public static OnRequestConversation e;

        public static void Trigger(Conversation conversation)
        {
            e.conversation = conversation;
            EventManager.TriggerEvent<OnRequestConversation>(e);
        }
    }
}