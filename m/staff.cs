using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using MySql.Data.EntityFrameworkCore.DataAnnotations;using Microsoft.EntityFrameworkCore;
using mercury.business;
using Newtonsoft.Json;

namespace mercury.model
{
    public class staff
    {
        public static string prefix { get; } = "staff";
        public string id { get; set; }
        public string avatar { set; get; }
        public string name { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public long dt_register { get; set; } = 0;
        public long dt_last_login { get; set; } = 0;
        public long dt_login_fail { get; set; } = 0;
        public int logins_failed_count { get; set; } = 0;
        public int status { get; set; }
        public string key { set; get; }
        public string hash { set; get; }
        public staff() {}
    }
}