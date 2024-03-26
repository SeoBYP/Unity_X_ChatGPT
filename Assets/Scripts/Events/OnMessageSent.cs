namespace Events
{
    public struct OnMessageSent
    {
        private static OnMessageSent e;

        public static void Trigger()
        {
            EventManager.TriggerEvent<OnMessageSent>(e);
        }
    }
}