public static class Constants
{
    public static class Events
    {
        public static string AGENT_ADDED_TO_FLOCK = "FLockChanged";
        public static string AGENT_DIED = "AgentDied";
        public static string SLIDER_AGGRESSION_CHANGED = "SliderAggressionChanged";
        public static string SLIDER_SPREAD_CHANGED = "SliderSpreadChanged";
        public static string FLOCK_DEPLETED = "FlockDepleted";
        public static string FACTION_DEPLETED = "FactionDepleted";
        public static string FRENZY_ACTIVATED = "FrenzyActivated";
    }
    public static class Tags { }
    public static class Layers { }

    public static class Factions
    {
        public static string ZOMBIES = "zombies";
        public static string HUMANS = "humans";
    }

    //TODO: Create parent static class Animation, then subclasses for Triggers, Bools, etc.
    public static class AnimationTriggers
    {
        public static string FIRE_WEAPON = "FireWeapon";
        public static string RUN = "Run";
        public static string IDLE = "Idle";
        public static string DIE = "Die";
        public static string ATTACK = "Attack";
    }
}
