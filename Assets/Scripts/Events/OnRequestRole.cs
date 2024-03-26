using Rolos;

namespace Events
{
    public struct OnRequestRole
    {
        public Roles roles;

        public OnRequestRole(Roles roles)
        {
            this.roles = roles;
        }

        public static OnRequestRole e;
        
        public static void Trigger(Roles roles)
        {
            e.roles = roles;
            EventManager.TriggerEvent<OnRequestRole>(e);
        }
    }
}