namespace Events
{
    public struct OnNoMoreChat
    {
        private static OnNoMoreChat e;

        public static void Trigger()
        {
            EventManager.TriggerEvent<OnNoMoreChat>(e);
        }
    }
}