namespace mercury.model
{
    public class attachment
    {
        public static string prefix { get; } = "attachment";
        public string id { get; set; }
        public string title { get; set; }
        public string ext { get; set; }
        public string data { get; set; }
        public long dt { get; set; }
        public attachment() {}
    }
}