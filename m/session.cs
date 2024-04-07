namespace mercury.model
{
    public class session
    {
        public static string prefix { get; } = "session";
        public string id { get; set; }
        public string user_id { get; set; }
        public string code { get; set; }
        public string device { get; set; }
        public string location { get; set; }
        public string token { get; set; }
        public long dt { get; set; }
        public long dt_active { get; set; }
        public int status { get; set; }
        public session() {}
    }
}