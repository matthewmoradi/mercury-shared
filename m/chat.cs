namespace mercury.model
{
    public class chat
    {
        public static string prefix { get; } = "chat";
        public string id { get; set; }
        public string user_1_id { get; set; }
        public string user_2_id { get; set; }
        public string group_id { get; set; }
        public chat() {}
    }
}