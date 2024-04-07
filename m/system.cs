
using mercury.business;

namespace mercury.model
{
    [Serializable]
    public class sys_update
    {
        public string file_name { get; set; }
        public string value_base64 { get; set; }
        public string type { get; set; }
    }

    [Serializable]
    public class sys_keyvalue
    {
        public string key { get; set; }
        public string value { get; set; }
        public string color { get; set; }
        public sys_keyvalue(string key, object value, string color = "")
        {
            this.key = key.Replace("_", " ");
            this.value = value.ToString();
            this.color = color;
        }
    }
    public class sys_section
    {
        public string title;
        public string type;
        public string val_plain;
        public List<sys_keyvalue> keyvalues { get; set; }
        public sys_section(string title, string type, List<sys_keyvalue> keyvalues, string val_plain = "")
        {
            this.title = title;
            this.type = type;
            this.keyvalues = keyvalues;
            this.val_plain = val_plain;
        }
    }
    [Serializable]
    public class system_resource_log
    {
        public long dt { get; set; }
        public long dt_l { get; set; }
        public List<sys_section> sections = new List<sys_section>();
        public List<sys_section> charts = new List<sys_section>();
        public List<sys_section> sections_ctl = new List<sys_section>();
        public system_resource_log()
        {
            dt = stringify.dttol(DateTime.Now);
            dt_l = stringify.dttol(DateTime.Now);
        }

    }

    [Serializable]
    public class system_exception_log
    {
        public string id { get; set; }
        public string message { get; set; }
        public string stack { get; set; }
        public string des { get; set; }
        public long dt { get; set; }
        public long dt_l { get; set; }
        public system_exception_log()
        {
            dt = stringify.dttol(DateTime.Now);
            dt_l = stringify.dttol(DateTime.Now);
        }
    }

    [Serializable]
    public class system_exception_log_min
    {
        public string id { get; set; }
        public string message { get; set; }
        public string des { get; set; }
        public string dt { get; set; }
        public string dt_l { get; set; }
        public system_exception_log_min(system_exception_log ex)
        {
            this.id = ex.id;
            this.message = ex.message;
            this.des = ex.des;
            this.dt = stringify.ltodt(ex.dt).ToString(entity.dt_format);
            this.dt_l = stringify.ltodt(ex.dt_l).ToString(entity.dt_format);
        }
    }
}