namespace AutoPlusPlusMVC
{
    public static class Global
    {
        public enum type
        {
            administrator = 1, moderator = 3, inspector = 2, user = 4
        }

        public enum abilities
        {
            allow_all = 1, dont_allow_topic_creation = 2
        }
    }
}
