using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mercury.business;

namespace mercury.model
{
    public class contact
    {
        public static string prefix { get; } = "contact";
        public string id { get; set; }
        public string user_1_id { get; set; }
        public string user_2_id { get; set; }
        public contact() {}
    }
}