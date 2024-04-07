namespace mercury.model
{
    public class group
    {
        public static string prefix { get; } = "group";
        public string id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string creator_id { get; set; }
        public long dt_created { get; set; }
        public int type { get; set; }
        public group() {}
    }
}