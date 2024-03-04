using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using mercury.business;

namespace mercury.model
{
    public class dto
    {
        [Serializable]
        public class dateTime
        {
            public int year { get; set; } = DateTime.Now.Year;
            public int month { get; set; } = DateTime.Now.Month;
            public int day { get; set; } = DateTime.Now.Day;
            public int hour { get; set; } = 0;
            public int min { get; set; } = 0;
            public int sec { get; set; } = 0;
        }

        [Serializable]
        public class msg
        {
            public string code { get; set; }
            public string str { get; set; }
            public string data { get; set; }
            public bool success { get; set; }
            public int? size { get; set; } = 0;
            public static string response_code_valid = "200";
            public static string response_code_invalid = "500";
            public static string response_code_redirect = "301";
            public static string response_code_logout = "302";
            public static string response_code_refresh = "303";
            public msg() { }
            public msg(string _code, string _str, string _data, bool _success, int? size = null)
            {
                this.code = _code;
                this.str = _str;
                this.data = _data;
                this.success = _success;
                this.size = size != null ? size : 0;
            }
            public msg(string _code, string _str, string _data, int? size = null)
            {
                this.code = _code;
                this.str = _str;
                this.data = _data;
                this.success = true;
                this.size = size != null ? size : 0;
            }
            public msg(string _code, string _str, int _data, bool _success)
            {
                this.code = _code;
                this.str = _str;
                this.data = _data.ToString();
                this.success = _success;
                this.size = 0;
            }
            public msg(string _code, string _str, int _data)
            {
                this.code = _code;
                this.str = _str;
                this.data = _data.ToString();
                this.success = true;
                this.size = 0;
            }
            public static msg success_data(string data)
            {
                return new dto.msg(response_code_valid, "", data, true);
            }
            public static msg success_(string str = null)
            {
                if (str == null)
                    str = message_sys.success;
                return new dto.msg(response_code_valid, str, "", true);
            }
            public static msg fail_(string str = null)
            {
                if (str == null)
                    str = message_sys.failed;
                return new dto.msg(response_code_valid, str, "", false);
            }
            public static msg error_500()
            {
                return new dto.msg(response_code_invalid, message_sys.NOT_OK, "", false);
            }
            public static msg error_unauth()
            {
                return new dto.msg(response_code_logout, message_sys.NOT_AUTHED, "", false);
            }
        }

        [Serializable]
        public class clock
        {
            public int min { get; set; }
            public long sec { get; set; }
            public clock(DateTime from)
            {
                min = (int)(DateTime.Now - from).TotalMinutes;
                sec = (long)(DateTime.Now - from).TotalSeconds;
            }
        }

        [Serializable]
        public class chart
        {
            public List<long> x { get; set; }
            public List<long> y { get; set; }
        }
        public class section_dash
        {
            public string title;
            public string type;
            public List<keyvalue> keyvalues { get; set; }
            public section_dash(string title, string type, List<keyvalue> keyvalues)
            {
                this.title = title;
                this.type = type;
                this.keyvalues = keyvalues;
            }
        }

        [Serializable]
        public class keyvalue
        {
            public string key { get; set; }
            public string value { get; set; }
            public keyvalue(string key, string value)
            {
                this.key = key.Replace("_", " ");
                this.value = value;
            }
        }

        [Serializable]
        public class permission
        {
            public string key { get; set; }
            public bool g { get; set; }
            public bool g_ { get; set; }
            public bool _ { get; set; }
            public bool _0 { get; set; }
            public permission(string key, bool g, bool g_, bool _, bool _0)
            {
                this.key = key;
                this.g = g;
                this.g_ = g_;
                this._ = _;
                this._0 = _0;
            }
        }

        public class zp_res
        {
            // {\"Status\":-11,\"Authority\":\"\",\"errors\":{\"Description\":[\"The description field is required.\"]}}
            public string Status { get; set; }
            public string Authority { get; set; }
            public string errors { get; set; }
            public string RefID { get; set; }
            public zp_res()
            { }
        }
    }
}