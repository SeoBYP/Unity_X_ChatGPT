namespace Events
{
    public struct OnChatGPTMessageReceived
    {
        public string Message;

        public OnChatGPTMessageReceived(string message)
        {
            this.Message = message;
        }

        private static OnChatGPTMessageReceived e;

        public static void Trigger(string message)
        {
            e.Message = message;
            EventManager.TriggerEvent<OnChatGPTMessageReceived>(e);
        }
    }
}