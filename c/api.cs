using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using MimeKit;
using System.Net.Http.Headers;
using mercury.business;
using mercury.model;

namespace mercury.controller
{
    public class api : Controller
    {
        public static async Task<string> fetch(string url, string data)
        {
            try
            {
                WebClient wc = new WebClient();
                string res = "";
                if (!string.IsNullOrEmpty(data))
                    res = wc.UploadString(url, data);
                else
                    res = wc.DownloadString(url);
                return await Task.FromResult(res);
            }
            catch (Exception ex)
            {
                _sys.log(ex);
                return null;
            }
        }
        public static string post(string url, string data, Dictionary<string, string> headers = null)
        {
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET");
                if (headers != null)
                    foreach (KeyValuePair<string, string> header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                //
                StringContent cnt = new StringContent(data);
                string res = "";
                res = client.PostAsync(url, cnt).Result.Content.ReadAsStringAsync().Result;
                return res;
            }
            catch (Exception ex)
            {
                // _sys.log(ex);
                System.Console.WriteLine(ex);
                return null;
            }
        }
        public static string post(string url, Dictionary<string, string> data, Dictionary<string, string> headers = null)
        {
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET");
                if (headers != null)
                    foreach (KeyValuePair<string, string> header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                //
                FormUrlEncodedContent cnt = new FormUrlEncodedContent(data);
                string res = "";
                res = client.PostAsync(url, cnt).Result.Content.ReadAsStringAsync().Result;
                return res;
            }
            catch (Exception ex)
            {
                // _sys.log(ex);
                System.Console.WriteLine(ex);
                return null;
            }
        }
        public static string get(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET");
                if (headers != null)
                    foreach (KeyValuePair<string, string> header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                //
                string res = "";
                res = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                return res;
            }
            catch (Exception ex)
            {
                // _sys.log(ex);
                return null;
            }
        }
        public static string post_form_data(string url, MultipartFormDataContent form, Dictionary<string, string> headers = null)
        {
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET");
                if (headers != null)
                    foreach (KeyValuePair<string, string> header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                string res = client.PostAsync(url, form).Result.Content.ReadAsStringAsync().Result;
                return res;
            }
            catch (Exception ex)
            {
                // _sys.log(ex);
                return null;
            }
        }
        public static bool email_mime(string email_to, string subject, string body, string email_from_display_name, string email_from_address)
        {
            MimeMessage message = new MimeMessage();
            try
            {
                message.From.Add(new MailboxAddress(email_from_display_name, email_from_address));
                message.To.Add(new MailboxAddress(email_to, email_to));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_io._config_value("smtp_host"), int.Parse(_io._config_value("smtp_host_port")), MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate(_io._config_value("email_addr"), _io._config_value("email_pwd"));
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
        }
    }
}