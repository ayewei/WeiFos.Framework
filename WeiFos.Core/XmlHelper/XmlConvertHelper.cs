using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WeiFos.Core.XmlHelper
{
    /// <summary>
    /// xml序列化工具类
    /// @author yewei 
    /// @date 2013-11-04
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlConvertHelper 
    {

        /// <summary>
        /// 获取该对象xml 格式
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(t.GetType());

                XmlWriterSettings settings = new XmlWriterSettings();

                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = Encoding.UTF8;
                settings.IndentChars = "    ";

                // 不生成声明头
                settings.OmitXmlDeclaration = true;

                // 强制指定命名空间，覆盖默认的命名空间。
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, t, namespaces);
                    writer.Close();
                }
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }



        /// <summary>
        /// xml字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string xmlString)
        {
            T t = default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            //使用 StringReader 读取，StreamReader 读取会有非法字符
            using (TextReader reader = new StringReader(xmlString))
            {
                t = (T)serializer.Deserialize(reader);
            }
            return t;  
        }


        #region dictionary与XmlDocument相互转换

        /// <summary>
        /// 字典转为xml字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string DictionaryToXmlString(Dictionary<string, string> dic)
        {
            StringBuilder xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            foreach (string key in dic.Keys)
            {
                xmlString.Append(string.Format("<{0}><![CDATA[{1}]]></{0}>", key, dic[key]));
            }
            xmlString.Append("</xml>");
            return xmlString.ToString();
        }

        /// <summary>
        /// xml字符串转换为字典
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlToDictionary(string xmlString)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            //防XXE漏洞攻击
            document.XmlResolver = null;
            //加载资源
            document.LoadXml(xmlString);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            var nodes = document.FirstChild.ChildNodes;

            foreach (System.Xml.XmlNode item in nodes)
            {
                dic.Add(item.Name, item.InnerText);
            }

            return dic;
        }


        #endregion

    }

}
