using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using mercury.model;
using mercury.business;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace mercury
{
    public static class _sys
    {
        private static List<system_exception_log> system_exception_logs = new List<system_exception_log>();
        private const string update_file_suffix = ".mat_update";

        
        public static system_exception_log desr(string d)
        {
            try
            {
                return JsonConvert.DeserializeObject<system_exception_log>(d);
            }
            catch { return null; }
        }
        public static IEnumerable<system_exception_log> get_exceptions()
        {

            system_exception_logs = _io.get_logs_sys().Take(100).Select(x => desr(x)).Where(x => x != null).ToList();
            return system_exception_logs;
        }
        public static IEnumerable<system_exception_log> get_exceptions(out int n, int skip, int take, bool asc = false, string sort_by = "id", string s = null)
        {
            var logs = _io.get_logs_sys();
            n = logs.Count;
            var propertyInfo = typeof(system_exception_log).GetProperty(sort_by);
            if (propertyInfo == null)
                propertyInfo = typeof(system_exception_log).GetProperty("id");
            var query = get_exceptions();
            if (!string.IsNullOrEmpty(s))
                query = query.Where(x => x.message.Contains(s) || (x.stack != null && x.stack.Contains(s)));
            if (asc)
                query = query.OrderBy(x => propertyInfo.GetValue(x, null));
            else
                query = query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            return query.Skip(skip).Take(take);
        }
        public static system_exception_log get_exceptions_(string id)
        {
            return system_exception_logs.FirstOrDefault(x => x.id == id);
        }
        private static List<Exception> Exceptions = new List<Exception>();
        public static async void log(Exception ex, string des = null)
        {
            if (Exceptions.Count > 500)
            {
                Console.WriteLine("log stopped");
                return;
            }
            Exceptions.Add(ex);
            var similars = Exceptions.Where(x => x.Message == ex.Message).Count();
            if (similars > 25)
                return;
            string message = ex.Message;
            if (similars == 24)
                message = "(Duplicattion Stoped) -> " + ex.Message;
            Console.WriteLine(ex);
            try
            {
                if (_io.get_logs_sys_count() > 1000)
                    _io.log_sys_pop();
                system_exception_log _system_exception_log = new system_exception_log();
                _system_exception_log.id = entity.id_new;
                _system_exception_log.des = des;
                _system_exception_log.message = ex.Message;
                _system_exception_log.stack = ex.StackTrace;
                //
                if (ex.InnerException != null)
                {
                    _system_exception_log.stack += Environment.NewLine + Environment.NewLine + Environment.NewLine + "<br>";
                    _system_exception_log.stack += " --------------------------------------------------------------- ";
                    _system_exception_log.stack += "<h1 class='ex_msg'>" + ex.InnerException.Message + "</h1><br>";
                    _system_exception_log.stack += Environment.NewLine + Environment.NewLine + "<br>";
                    _system_exception_log.stack += "<p>" + ex.InnerException.StackTrace + "</p>";
                }
                _io.save_log_sys(_system_exception_log.id, JsonConvert.SerializeObject(_system_exception_log));
            }
            catch (Exception ex_)
            {
                System.Console.WriteLine(ex_);
            }
        }
        public static dto.msg delete(string id)
        {
            try
            {
                _io.del_log_sys(id);
                get_exceptions();
                return new dto.msg("200", "Ok", "", true);
            }
            catch (Exception ex)
            {
                _sys.log(ex);
                return new dto.msg("500", ex.Message, "", true);
            }
        }


        public static bool is_unix()
        {
            var is_unix_ = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            return is_unix_;
        }
        public static system_resource_log get_resource_log()
        {
            string step = "0";
            var str_free = "";
            var str_htop = "";
            var str_df = "";
            var str_ctl = "";
            system_resource_log ret = new system_resource_log();
            try
            {
                if (!is_unix())
                    return ret;
                // HTOP
                var p_htop = new ProcessStartInfo("htop");
                p_htop.FileName = "/bin/bash";
                p_htop.RedirectStandardOutput = true;
                Process process_htop = Process.Start(p_htop);
                Task.Delay(3000).Wait();
                str_htop = process_htop.StandardOutput.ReadToEnd();
                process_htop.Dispose();
                step = "str_htop";
                // FREE
                var p_free = new ProcessStartInfo("free -m");
                p_free.FileName = "/bin/bash";
                p_free.Arguments = "-c \"free -m\"";
                p_free.RedirectStandardOutput = true;
                Process process_free = Process.Start(p_free);
                str_free = process_free.StandardOutput.ReadToEnd();
                process_free.Dispose();
                step = "str_free";
                //
                // DF
                var p_df = new ProcessStartInfo("df -m");
                p_df.FileName = "/bin/bash";
                p_df.Arguments = "-c \"df -m\"";
                p_df.RedirectStandardOutput = true;
                Process process_df = Process.Start(p_df);
                str_df = process_df.StandardOutput.ReadToEnd();
                process_df.Dispose();
                step = "str_df";
                // SYSTEMCTL
                string p = "systemctl status " + _io._config_value("sys_systemctl_d_name");
                var p_ctl = new ProcessStartInfo(p);
                p_ctl.FileName = "/bin/bash";
                p_ctl.Arguments = "-c \"" + p + " \"";
                p_ctl.RedirectStandardOutput = true;
                Process process_ctl = Process.Start(p_ctl);
                str_ctl = process_ctl.StandardOutput.ReadToEnd();
                process_ctl.Dispose();
                step = "str_ctl";
                //
                // string str_htop = _io.read("htop.txt");
                // string str_df = _io.read("df.txt");
                // string str_free = _io.read("free.txt");
                // string str_ctl = _io.read("htop.txt");
                //
                var lines_free = str_free.Split("\n");
                var lines_htop = str_htop.Split("\n");
                var mem = lines_free[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                ret = new system_resource_log();
                List<sys_keyvalue> mem_data_pie = new List<sys_keyvalue>();
                List<sys_keyvalue> cpu_data_pie = new List<sys_keyvalue>();
                List<sys_keyvalue> fs_data_pie = new List<sys_keyvalue>();
                List<sys_keyvalue> fs_data_bar = new List<sys_keyvalue>();
                List<sys_keyvalue> kernel_data = new List<sys_keyvalue>();
                List<sys_keyvalue> update_data = new List<sys_keyvalue>();
                kernel_data.Add(new sys_keyvalue("mem_total", double.Parse(mem[1]), "#34495e")); step = "mem_total";
                mem_data_pie.Add(new sys_keyvalue("mem_used", double.Parse(mem[2]), "#9b59b6")); step = "mem_used";
                mem_data_pie.Add(new sys_keyvalue("mem_free", double.Parse(mem[3]), "#bdc3c7")); step = "mem_free";
                mem_data_pie.Add(new sys_keyvalue("mem_aviable", double.Parse(mem[6]), "#16a085")); step = "mem_aviable";
                //
                kernel_data.Add(new sys_keyvalue("uptime", stringify.val_line(lines_htop, "Uptime: ", "", ", ", 0))); step = "uptime";
                kernel_data.Add(new sys_keyvalue("tasks", int.Parse(stringify.val_line(lines_htop, "Tasks: ", "", ", ", 0)))); step = "tasks";
                string threads = stringify.val_line(lines_htop, "Tasks: ", "", ";", 0);
                kernel_data.Add(new sys_keyvalue("threads", int.Parse(stringify.val_line(threads, ", ", "", " thr", 0)))); step = "threads";
                //
                string[] lines_app = lines_htop.Where(x => x.Contains(_io._config_value("sys_instance_name"))).ToArray();
                kernel_data.Add(new sys_keyvalue("threads_app", lines_app.Count())); step = "threads_app";
                List<string[]> htop_table_all = terminal_to_list_2d(lines_htop); step = "htop_table_all";
                List<string[]> htop_table_app = terminal_to_list_2d(lines_app); step = "htop_table_app";
                // index: 8-9 = cpu, mem
                string[] mem_app = htop_table_app.Where(x => x.Length >= 10 && stringify.is_num(x[9])).FirstOrDefault();
                string mem_app_ = "";
                if (mem_app != null && mem_app.Length >= 9)
                    mem_app_ = mem_app[9];
                mem_data_pie.Add(new sys_keyvalue("mem_app", mem_app_, "#2c3e50")); step = "mem_app";
                float cpu_pct_used = htop_table_all.Where(x => x.Length >= 10 && stringify.is_num(x[8])).Select(x => float.Parse(x[8])).Sum();
                float cpu_pct_app = htop_table_app.Where(x => x.Length >= 10 && stringify.is_num(x[8])).Select(x => float.Parse(x[8])).Sum();
                cpu_data_pie.Add(new sys_keyvalue("cpu_pct_free", 100 - cpu_pct_used, "#ecf0f1")); step = "cpu_pct_free";
                cpu_data_pie.Add(new sys_keyvalue("cpu_pct_other", cpu_pct_used - cpu_pct_app, "#34495e")); step = "cpu_pct_other";
                cpu_data_pie.Add(new sys_keyvalue("cpu_pct_app", cpu_pct_app, "#2980b9")); step = "cpu_pct_app";
                //
                var lines_fs = str_df.Split("\n");
                List<string[]> fss = lines_fs.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToList();
                string[] fs = fss.Where(x => stringify.is_num(x[1]) && x[0] == _io._config_value("sys_fs_main")).First();
                kernel_data.Add(new sys_keyvalue("fs_used_pct", int.Parse(fs[4].Replace("%", "")) + "%", "#c0392b")); step = "fs_used_pct";
                kernel_data.Add(new sys_keyvalue("fs_size(GB)", (double.Parse(fs[1]) / 1024).ToString("0.00"), "#c0392b")); step = "fs_size(GB)";
                double fs_used = double.Parse(fs[2]);
                double fs_app = _io.get_dir_size_kb("") / 1024;
                fs_data_pie.Add(new sys_keyvalue("fs_used(MB)", fs_used - fs_app, "#9b59b6")); step = "fs_used(MB)";
                fs_data_pie.Add(new sys_keyvalue("fs_used_app(MB)", fs_app, "#2c3e50")); step = "fs_used_app(MB)";
                fs_data_pie.Add(new sys_keyvalue("fs_aviable(MB)", double.Parse(fs[3]), "#3498db")); step = "fs_aviable(MB)";
                kernel_data.Add(new sys_keyvalue("fs_used_app(MB)", (fs_app / 1).ToString("0.00"), "#2c3e50")); step = "fs_used_app(MB)";
                //
                update_data.Add(new sys_keyvalue("version", entity.version, "#2c3e50")); step = "version";
                update_data.Add(new sys_keyvalue("db_size(MB)", (_io.get_file_size_kb("mat.db") / 1024).ToString("0.00"), "#2c3e50")); step = "db_size(MB)";
                //
                ret.charts.Add(new sys_section("mem", "pie", mem_data_pie));
                ret.charts.Add(new sys_section("cpu", "pie", cpu_data_pie));
                ret.charts.Add(new sys_section("fs", "pie", fs_data_pie));
                ret.sections.Add(new sys_section("kernel", "labels", kernel_data));
                ret.sections.Add(new sys_section("matryoshka", "labels", update_data));
                //
                ret.sections_ctl.Add(new sys_section("journal", "plain", null, str_ctl));
                return ret;
            }
            catch (Exception ex)
            {
                _sys.log(ex, step);
                return ret;
            }
        }
        public static bool cmd(string commond)
        {
            if (commond == "update")
                update();
            return true;
        }
        public static bool update_recv(string body_base64_enc_zip)
        {
            try
            {
                _io.write("", "update_" + Guid.NewGuid().ToString() + update_file_suffix, Encoding.UTF8.GetBytes(body_base64_enc_zip));
                return true;
            }
            catch (Exception ex)
            {
                log(ex);
                return false;
            }
        }
        private static bool update()
        {
            string file_update = _io.search("", update_file_suffix).FirstOrDefault();
            if (string.IsNullOrEmpty(file_update))
                return false;
            string body = stringify.unzip(Convert.FromBase64String(File.ReadAllText(file_update)));
            body = stringify.decrypt(body, entity.key_enc);
            XmlSerializer serializer = new XmlSerializer(typeof(List<sys_update>));
            MemoryStream stream = new MemoryStream();
            stream.Read(Encoding.UTF8.GetBytes(body));
            List<sys_update> sys_updates = (List<sys_update>)serializer.Deserialize(stream);
            // replace
            foreach (sys_update update_file in sys_updates)
            {
                // if (update_file.type == "root")
                //     _io.write("",)
            }
            var p_ctl = new ProcessStartInfo("sudo systemctl restart " + entity.bin_name);
            p_ctl.FileName = "/bin/bash";
            p_ctl.RedirectStandardOutput = true;
            return true;
        }
        private static List<string[]> terminal_to_list_2d(string[] lines)
        {
            List<string[]> ret = new List<string[]>();
            foreach (var line in lines)
                ret.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            return ret;
        }
    }
}