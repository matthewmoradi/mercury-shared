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
    public class ctrl_default : Controller
    {
        public static string cookie_get(HttpRequest request, string key)
        {
            if (request.Cookies.ContainsKey(key))
                return request.Cookies[key];
            return null;
        }
        private ActionResult _500(string str = "NOT OK")
        {
            return Json(new dto.msg("500", str, ""));
        }
        
        public static void cookie_set(HttpResponse response, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(15);
            // option.Domain = "localhost";
            response.Cookies.Append(key, value, option);
        }
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