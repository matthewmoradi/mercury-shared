namespace mercury.model
{
    public class preference
    {
        public static string prefix { get; } = "preference";
        public string app_name { get; set; }
        public preference(string app_name)
        {
            this.app_name = app_name;
        }
    }
}