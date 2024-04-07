namespace mercury.model
{
    public class language
    {
        public static string prefix { get; } = "language";
        public string id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string iso { get; set; }
        public string direction { get; set; } = "ltr";
        public bool def { get; set; } = false;
        public bool active { get; set; } = true;
        public long dt_update { get; set; }
        public word[] words { get; set; } = new word[0];
        // public Dictionary<string, string> dictionary { get; set; }
    }
    public class word
    {
        public string key { get; set; } //uniqe
        public string value { get; set; }
        public long dt_update { get; set; }
        public long dt_used { get; set; }
        public long count_used { get; set; }
        public word() { }
    }
}