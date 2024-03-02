using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using mercury.model;

namespace mercury.business
{
    public class email
    {
        public bool mail_sent {get; set;} = false;
        public bool mail_sent_success {get; set;} = false;
        public string error {get; set;} = "";
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string id = (string) e.UserState;
            if (e.Cancelled) {
                Console.WriteLine("code [{0}] Email  canceled.", id);
            }
            if (e.Error != null) {
                Console.WriteLine("code [{0}] Email, err: {1}", id, e.Error.ToString());
            } else {
                mail_sent_success = true;
            }
            mail_sent = true;
        }
        public void send(string id, string _to, string _subject, string _content)
        {
            SmtpClient client = new SmtpClient(email_config.mailServer, email_config.mailPort);
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(email_config.sender, email_config.password);
            // client.EnableSsl = true;
            MailAddress from = new MailAddress(email_config.sender, email_config.senderName, System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(_to);
            MailMessage message = new MailMessage(from, to);
            message.Body = _content;
            message.IsBodyHtml = true;
            message.BodyEncoding =  System.Text.Encoding.UTF8;
            message.Subject = _subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            client.SendAsync(message, id);
            // message.Dispose();
        }
    }
    public class email_config
    {
        public static string mailServer = "webmail.jamming.ir";
        public static int mailPort = 25;
        public static string senderName = "Tehran Music Nights";
        public static string sender = "no-reply@jamming.ir";
        public static string password = "Urgv809*";
    }

    public class template
    {
        public static string template_login_code(string name, string code)
        {
            string temp = "<div style=\"direction: rtl; font-family: monospace; font-size: 16px;\">" + "Hi " + name + "<br>";
            temp +=  "Login code: " + code;
            temp += "<br>" + "Do not give this code to anyone, even if they say they are from Mercury!";
            temp += "<br><br>" + "This code can be used to log in to your Telegram account. We never ask it for anything else.";
            temp += "<br><br>" + "If you didn't request this code by trying to log in on another device, simply ignore this message.";
            temp += "</div>";
            return temp;
        }
    }
}