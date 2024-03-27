namespace Events
{
    public struct OnUpdateScroll
    {
        private static OnUpdateScroll e;

        public static void Trigger()
        {
            EventManager.TriggerEvent<OnUpdateScroll>(e);
        }
    }
}