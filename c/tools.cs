using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mercury.model;
using mercury.business;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace mercury.controller
{
    public class ctrl_tools : Controller
    {
        #region tools
        public static string cookie_get(HttpRequest req, string key)
        {
            if (req.Cookies.ContainsKey(key))
                return req.Cookies[key];
            return null;
        }
        public static ActionResult _500(Controller c,string str = "NOT OK")
        {
            return c.Json(new dto.msg("500", str, ""));
        }

        public static void cookie_set(HttpResponse res, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(15);
            // option.Domain = "localhost";
            res.Cookies.Append(key, value, option);
        }
        public static ActionResult ret(Controller c, staff _staff, dto.msg obj)
        {
            string serd = JsonConvert.SerializeObject(obj);
            string ret = Convert.ToBase64String(Encoding.UTF8.GetBytes(serd));
            Console.WriteLine("Response Len: " + ret.Length);
            return c.Content(ret);
        }
        public static ActionResult ret(Controller c, dto.msg obj)
        {
            string serd = JsonConvert.SerializeObject(obj);
            // Console.WriteLine("Response Len: " + ret.Length);
            return c.Content(serd);
        }
        public static bool contains(Dictionary<string, string> dic, string[] keys)
        {
            foreach (var key in keys)
                if (!dic.ContainsKey(key))
                    return false;
            return true;
        }
        public static void cookie_set_user(HttpResponse res, user _user, string token)
        {
            cookie_set(res, "token", token, entity.cookie_expire_day_user);
            cookie_set(res, "user_id", _user.id, entity.cookie_expire_day_user);
        }
        public static void cookie_set_staff(HttpResponse res, staff _staff)
        {
            string token_ = stringify.calc_token(_staff);
            cookie_set(res, "token", token_, entity.cookie_expire_day_staff);
            cookie_set(res, "staff_id", _staff.id, entity.cookie_expire_day_staff);
        }
        public static bool is_token_active(List<session> sessions, string user_id, string token)
        {
            return sessions.Any(x => x.user_id == user_id && x.token == token && x.status == (int)entity.enum_session_statuses.active);
        }
        public static user get_user(HttpRequest req, List<user> users, List<session> sessions)
        {
            string user_id = cookie_get(req, "user_id");
            string token = cookie_get(req, "token");
            if (user_id == null || token == null)
                return null;
            user _user = users.FirstOrDefault(x => x.id == user_id);
            if (_user == null || !is_token_active(sessions, user_id, token))
                return null;
            return _user;
        }
        #endregion
        public static user validate(user _user, string token, string prm, bool g, bool g_, bool _, bool _0)
        {
            // if (token.Replace(' ', '+') != stringify.calc_token(_user))
            //     return null;
            // dto.permission perm = _user.prm_.Where(x => x.key == prm).FirstOrDefault();
            // if (perm == null)
            //     return null;
            // if (g && !perm.g)
            //     return null;
            // else if (g_ && !perm.g_)
            //     return null;
            // else if (_ && !perm._)
            //     return null;
            // else if (_0 && !perm._0)
            //     return null;
            return _user;
        }
    }
}