using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection;
using Newtonsoft.Json;
using mercury.business;

namespace mercury.model
{
    public class entity
    {
        public const int max_message_len = 2048;
        public const int max_fails = 5;
        public const int max_fails_staffs = 3;
        public const int L_K_X = 8;
        public const int cookie_expire_day_staff = 7;
        public const int cookie_expire_day_user = 365;
        public const string bin_name = "mercury";
        public const string version = "1.0";
        public const string salt = "OD09PT09RCB+fn5+fiAhQCMkJV4mKigpXysrXykoKiZeJSQjQCE5ODc2NTQzMjE=";
        public const string key_enc = "OD09PT09RCB+fn5+fiAhQ-----V4mKigpXysrXykoKiZeJSQjQ~~~~5ODc2NTQzMjE=";
        public const string date_format = "yyyy/MM/dd";
        public const string time_format = "HH:mm";
        public const string weekday_format = "ddd";        
        public const string date_format_html = "yyyy-MM-dd";
        public const string dt_format = "yyyy/MM/dd HH:mm:ss";
        public static _const const_ = new _const();
        
        public static string url_attachment_thumb(string attachment_id)
        {
            return "fs?_300=" + attachment_id;
        }
        public static string url_attachment_cover(string attachment_id)
        {
            return "fs?_800=" + attachment_id;
        }
        public static string url_attachment_org(string attachment_id)
        {
            return "fs?o=" + attachment_id;
        }
        public static bool isDigit(char c)
        {
            return c >= 48 && c <= 57;
        }
        public static string dirtyNum_to_num(string dirtyNum)
        {
            string str_ = "";
            for (int i = 0; i < dirtyNum.Length; i++) {
                if (isDigit(dirtyNum[i])) str_ += dirtyNum[i];
            }
            return str_;
        }
        public static string sep3(string str)
        {
            if (str == "" || str.Length == 0) return "";
            str = (dirtyNum_to_num(str));
            var set_i = str.Length % 3;
            var res = "";
            for (var i = 0; i < str.Length; i++) {
                if (i != 0 && i % 3 == set_i) res += ",";
                res += str[i];
            }
            return res;
        }
        public static string sep3(int num)
        {
            string res = sep3(num.ToString());
            if(num < 0) return "-" + res;
            return res;
        }
        public static string id_new
        {
            get
            {
                var ticks = new DateTime(2016, 1, 1).Ticks;
                var ans = DateTime.Now.Ticks - ticks;
                string uniqueId = ans.ToString("x");
                return uniqueId;
            }
        }
        public static string code_new
        {
            get
            {
                return new Random().Next(10000, 99999).ToString();
            }
        }
        public static long id_new_
        {
            get
            {
                var ticks = new DateTime(2016, 1, 1).Ticks;
                var ans = DateTime.Now.Ticks - ticks;
                string uniqueId = ans.ToString("x");
                long part2 = Convert.ToInt64(uniqueId, 16);
                return part2;
            }
        }

        public static List<dto.permission> permissions
        {
            get
            {
                List<dto.permission> ret = new List<dto.permission>();
                ret.Add(new dto.permission("dashboard", false, false, false, false));
                ret.Add(new dto.permission("attachments", false, false, false, false));
                ret.Add(new dto.permission("users", false, false, false, false));
                ret.Add(new dto.permission("consts", false, false, false, false));
                ret.Add(new dto.permission("sys_logs", false, false, false, false));
                return ret;
            }
        }
        
        [Serializable]
        public enum enum_login_flags
        {
            [JsonProperty]
            ok = 1,
            [JsonProperty]
            fail = 2,
        }
        public enum enum_user_flags
        {
            [JsonProperty]
            ok = 0,
            [JsonProperty]
            block = 1,
        }
        public enum enum_staff_flags
        {
            [JsonProperty]
            ok = 0,
            [JsonProperty]
            block = 1,
        }
        public enum enum_session_statuses
        {
            [JsonProperty]
            pending = 0,
            [JsonProperty]
            active = 1,
            [JsonProperty]
            expired = 2,
        }
        public enum enum_message_types
        {
            [JsonProperty]
            text = 0,
            [JsonProperty]
            photo = 1,
            [JsonProperty]
            audio = 3,
            [JsonProperty]
            video = 4,
            [JsonProperty]
            voice = 5,
            [JsonProperty]
            sticker = 6,
            [JsonProperty]
            gif = 7,
            [JsonProperty]
            file = 8,
        }
    }
}