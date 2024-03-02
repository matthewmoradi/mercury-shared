using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.IO.Compression;
using mercury.model;
using System.Text.RegularExpressions;

namespace mercury.business
{
    public class stringify
    {
        public static string linkify(string SearchText)
        {
            // Regex.Replace(_body, @"^(http|https)\://[a-zA-Z0-9\-\.]+" + @"\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?" +
            //                     @"([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$",
            //                     "<a href=\"$1\">$1</a>");
            Regex regx = new Regex(@"\b(((\S+)?)(@|mailto\:|(news|(ht|f)tp(s?))\://)\S+)\b", RegexOptions.IgnoreCase);
            SearchText = SearchText.Replace("&nbsp;", " ");
            MatchCollection matches = regx.Matches(SearchText);
            foreach (Match match in matches)
            {
                if (match.Value.StartsWith("http"))
                { // if it starts with anything else then dont linkify -- may already be linked!
                    SearchText = SearchText.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
                }
            }
            return SearchText;
        }
        public static string encrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (string.IsNullOrEmpty(key))
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(crypt(str, key)));
        }
        public static string decrypt(string str_base64, string key)
        {
            try
            {
                return crypt(Encoding.UTF8.GetString(Convert.FromBase64String(str_base64)), key);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                return null;
            }
        }
        private static string crypt(string bytes, string key)
        {
            int n = bytes.Length;
            char[] ret = new char[n];
            for (int i = 0; i < n; i++)
                ret[i] = (char)(bytes[i] ^ key[i % key.Length]);
            return new string(ret);
        }
        public static DateTime ltodt(long dt)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(dt);
            return date;
        }
        public static long dttol(DateTime dt)
        {
            if (dt == null) return 0;
            return (long)dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
        public static string from_base64(string base64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
        public static string to_base64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        public static dto.dateTime toDateTime_dto(string dt)
        {

            if (!string.IsNullOrEmpty(dt) && (!dt.Contains("0:0:0") || !dt.Contains("00:00:00")))
            {
                dt.Trim();
                dt = $"{dt} 0:0:0";
            }
            dto.dateTime dateTime = new dto.dateTime();
            try
            {
                var date = dt.Remove(dt.IndexOf(' ')).Split('/').Where(x => !string.IsNullOrEmpty(x)).ToList();
                var time = dt.Remove(0, dt.IndexOf(' ')).Split(':').Where(x => !string.IsNullOrEmpty(x)).ToList();
                dateTime.year = int.Parse(date[0]);
                dateTime.month = int.Parse(date[1]);
                dateTime.day = int.Parse(date[2]);
                dateTime.hour = int.Parse(time[0]);
                dateTime.min = int.Parse(time[1]);
                dateTime.sec = int.Parse(time[2]);
            }
            catch
            {

            }
            return dateTime;
        }

        public static string calc_token(user _user, string session_id)
        {
            if (_user == null)
                return null;
            string token = _hash.sha256(new string[]
            {
                _user.id, session_id, _user.username
            });
            return token;
        }
        public static string calc_token(staff _staff)
        {
            if (_staff == null)
                return null;
            string token = _hash.sha256(new string[]
            {
                _staff.id, "hi baby user", _staff.phone
            });
            return token;
        }
        public static string hash(user _user)
        {
            string hash = _hash.sha256(new string[] { _user.id, _user.name_first, _user.username, "EECBB0B6257CB2740E6D5134630C1AA563E73904891" });
            return hash;
        }
        public static string hash(staff _staff)
        {
            string hash = _hash.sha256(new string[] { _staff.id, _staff.email, _staff.phone, "EECBB0B6257CB2740E6D5134630C1AA563E73904891" });
            return hash;
        }
        public static string uid()
        {
            Task.Delay(100);
            var ticks = new DateTime(2016, 1, 1).Ticks;
            var ans = DateTime.Now.Ticks - ticks;
            var uniqueId = ans.ToString("x");
            return uniqueId;
        }
        private static void copyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
                dest.Write(bytes, 0, cnt);
        }

        public static byte[] zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    copyTo(msi, gs);
                return mso.ToArray();
            }
        }

        public static string unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    copyTo(gs, mso);
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string base64_resolve_for_bitmap(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty);
            sbText.Replace(" ", String.Empty);
            sbText.Remove(0, Image.LastIndexOf(',') + 1);
            return sbText.ToString();
        }
        public static string base64_get_suffix(string base64)
        {
            int d = base64.LastIndexOf(',');
            string ret = base64.Remove(d + 1, base64.Length - d - 1);
            return ret;
        }

        public static bool is_num(string num)
        {
            try { float.Parse(num); return true; }
            catch { return false; }
        }
        public static string val_line(string[] lines, string look_after, string remove_untile, string look_until, int skip)
        {
            List<string> vals = new List<string>();
            foreach (string line in lines)
            {
                string val = val_line(line, look_after, remove_untile, look_until, skip);
                if (!string.IsNullOrEmpty(val))
                    if (skip == 0)
                        return val;
                    else
                        vals.Add(val);
            }
            return vals.Skip(skip).FirstOrDefault();
        }
        public static string val_line(string line, string look_after, string remove_untile, string look_until, int skip)
        {
            string ret = "";
            int index_1 = line.IndexOf(look_after);
            int before = index_1 + look_after.Length - 1;
            if (line.Length <= before)
                return "";
            int len = line.Remove(0, before).IndexOf(look_until) - 1;
            if (index_1 < 0 || len < 0)
                return "";
            ret = line.Substring(index_1 + look_after.Length, len);
            return ret;
        }
    }
}