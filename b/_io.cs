using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.ComponentModel;
using mercury.model;

namespace mercury.business
{
    public class _io
    {
        public static string _config_value(string key)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + "/cnf";
                return File.ReadAllLines(path).Where(x => x.Contains(key + ":"))
                    .FirstOrDefault().Split(':').ToList().Where(x => !string.IsNullOrEmpty(x) && x != key).FirstOrDefault();
            }
            catch { return null; }
        }
        public static string image_resize(string base64, int size)
        {
            try
            {
                byte quality = 75;
                string base64_net = stringify.base64_resolve_for_bitmap(base64);
                byte[] bytes = Convert.FromBase64String(base64_net);
                MemoryStream s = new MemoryStream(bytes);
                var image = new Bitmap((Bitmap)Image.FromStream(s));
                int width, height;
                if (image.Width > image.Height)
                {
                    width = size;
                    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                }
                else
                {
                    height = size;
                    width = Convert.ToInt32(image.Width * size / (double)image.Height);
                }
                var resized = new Bitmap(width, height);
                var graphics = Graphics.FromImage(resized);
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, 0, 0, width, height);
                var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                MemoryStream s2 = new MemoryStream();
                resized.Save(s2, codec, encoderParameters);
                return Convert.ToBase64String(s2.ToArray());
            }
            catch (Exception x)
            {
                Console.WriteLine("_io: ");
                Console.WriteLine(x);
                return "";
            }
        }
        
        public static List<string> search(string dir_sub, string filename)
        {
            List<string> ret = new List<string>();
            string path = Directory.GetCurrentDirectory() + "/" + dir_sub;
            if (!string.IsNullOrEmpty(path))
                path += "/";
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
                if (file.Contains(filename))
                    ret.Add(file);
            return ret;
        }
        public static bool write(string dir_sub, string filename, byte[] bytes)
        {
            string path = Directory.GetCurrentDirectory() + "/" + dir_sub;
            if (!string.IsNullOrEmpty(path))
                path += "/";
            path += filename;
            File.WriteAllBytes(path, bytes);
            return true;
        }
        public static long get_dir_size_kb(string dir)
        {
            string dir_base = Directory.GetCurrentDirectory() + dir;
            long size = 0;
            var dirInfo = new DirectoryInfo(dir_base);
            foreach (FileInfo fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                size += fi.Length;
            return size / 1024;
        }
        public static long get_file_size_kb(string file_name)
        {
            string fn = Directory.GetCurrentDirectory() + "/" + file_name;
            if (!File.Exists(fn))
                return -1;
            return new FileInfo(fn).Length / 1024;
        }
        public static int get_logs_sys_count()
        {
            try
            {
                return Directory.GetFiles(Directory.GetCurrentDirectory() + "/logs_sys/").Count();
            }
            catch { return 0; }
        }
        public static List<string> get_logs_sys()
        {
            List<string> ret = new List<string>();
            try
            {
                string dir_base = Directory.GetCurrentDirectory() + "/logs_sys/";
                foreach (string d in Directory.GetFiles(dir_base))
                {
                    string cnt = stringify.unzip(File.ReadAllBytes(d));
                    cnt = stringify.decrypt(cnt, entity.key_enc);
                    ret.Add(cnt);
                }
            }
            catch (Exception ex)
            {
                //_sys.log(ex);
                return ret;
            }
            return ret;
        }
        public static string save_log_sys(string id, string cnt)
        {
            try
            {
                string dir_base = Directory.GetCurrentDirectory() + "\\logs_sys\\";
                string filename = id + ".mercury_log_sys";
                cnt = stringify.encrypt(cnt, entity.key_enc);
                File.WriteAllBytes(dir_base + filename, stringify.zip(cnt));
                return filename;
            }
            catch (Exception ex)
            {
                // _sys.log(ex);
                System.Console.WriteLine(ex.Message);
                return "err";
            }
        }
        public static bool log_sys_pop()
        {
            try
            {
                string dir_base = Directory.GetCurrentDirectory() + "/logs_sys/";
                DirectoryInfo info = new DirectoryInfo(dir_base);
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                int deleted = 0;
                foreach (FileInfo file in files.Take(50))
                {
                    if (file.LastWriteTimeUtc < DateTime.Now.ToUniversalTime().AddDays(-7))
                    {
                        File.Delete(dir_base + file.Name);
                        deleted++;
                    }
                }
                if (deleted == 0)
                    foreach (FileInfo file in files.Take(5))
                        File.Delete(dir_base + file.Name);
                return true;
            }
            catch
            {
                return false;
            }


        }
        public static void del_log_sys(string id)
        {
            try
            {
                string dir_base = Directory.GetCurrentDirectory() + "/logs_sys/";
                string filename = id + ".mercury_log_sys";
                File.Delete(dir_base + filename);
            }
            catch (Exception ex)
            {
                _sys.log(ex);
            }
        }
    }
}