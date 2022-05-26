using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeiFos.Core
{
    /// <summary>
    /// 记录日志
    /// </summary>
    public static class Logs
    {
        static object obj2 = new object();

        private static bool Create(string text, string folder, Type t)
        {
            try
            {
                bool lockTaken = false;

                try
                {
                    Monitor.Enter(obj2, ref lockTaken);
                    DateTime now = DateTime.Now;
                    string str = string.Format(@"{0}\Logs\{1}\{2}\", AppDomain.CurrentDomain.BaseDirectory, folder, now.ToString("yyyy-MM-dd")).Replace(@"\\", @"\");
                    string path = string.Format("{0}{1}.log", str, now.ToString("HH"));
                    Directory.CreateDirectory(str);
                    StreamWriter writer = null;
                    writer = File.Exists(path) ? File.AppendText(path) : new StreamWriter(path, false, Encoding.UTF8);

                    if (t != null)
                    {
                        writer.WriteLine("命名空间：" + t.Namespace);
                        writer.WriteLine("操作类名：" + t.Name);
                    }
                    writer.WriteLine("创建时间：" + now.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLine(text);
                    writer.WriteLine("--------------------------------------------------------------------");
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(obj2);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Write(string text)
        {
            return Write(text, "", null);
        }

        public static bool Write(string text, string folder)
        {
            return Create(text, folder, null);
        }

        public static bool Write(string text, Type t)
        {
            return Write(text, "", t);
        }

        public static bool Write<T>(T t, string folder, Type type)
        {
            return Create(Newtonsoft.Json.JsonConvert.SerializeObject(t), folder, type);
        }

        public static bool Write(string text, string folder, Type t)
        {
            return Create(text, folder, t);
        }
    }
}
