using System;
using System.Linq;
using mercury.business;
using Newtonsoft.Json;

namespace mercury.model
{
    public class user
    {
        public static string prefix { get; } = "user";
        public string id { get; set; }
        public string avatar { set; get; }
        public string name_first { get; set; }
        public string name_last { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public long dt_register { get; set; } = 0;
        public long dt_last_login { get; set; } = 0;
        public long dt_login_fail { get; set; } = 0;
        public int logins_failed_count { get; set; } = 0;
        public int status { get; set; }
        public string hash { set; get; }
        public user() {}
        public string get_name()
        {
            if(string.IsNullOrEmpty(this.name_first))
                return this.username;
            if(string.IsNullOrEmpty(this.name_last))
                return this.name_first;
            return this.name_first + " " + this.name_last;
        }
    }
}