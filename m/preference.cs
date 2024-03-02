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