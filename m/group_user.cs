namespace mercury.model
{
    public class group_user
    {
        public static string prefix { get; } = "group_user";
        public string id { get; set; }
        public string group_id { get; set; }
        public string user_id { get; set; }
        public string role { get; set; }
        public long dt_joined { get; set; }
        public string[] permissions { get; set; }
        public group_user() {}
    }
}