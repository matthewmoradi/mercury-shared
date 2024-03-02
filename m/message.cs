using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mercury.business;
using Newtonsoft.Json;

namespace mercury.model
{
    public class message
    {
        public static string prefix { get; } = "message";
        public string id { get; set; }
        public string user_id { set; get; }
        public string chat_id { get; set; }
        public string forward_id { get; set; } //forwarded from message id
        public string reply_id { get; set; } //in reply to message id
        public string text { get; set; }
        public string text_edited { get; set; }
        public string attachment_id { get; set; }
        public int type { get; set; } = 0;
        public bool has_link { get; set; } = false;
        public long dt_sent { set; get; }
        public long dt_update { set; get; }
    }
}