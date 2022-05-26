using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace WeiFos.Core.XmlHelper
{
    /// <summary>
    /// 站点缺省资源路径配置类
    /// @author yewei
    /// @date 2014-02-14
    /// </summary>
    public class ResXmlConfig
    {
        #region 单列模式  

        /*私有构造器，不能该类外部new对象*/
        private ResXmlConfig()
        {
            //配置文件名称
            string resXmlConfig = "ResXmlConfig.xml";

            string path = Directory.GetParent(resXmlConfig).FullName + "\\wwwroot\\Config\\" + resXmlConfig;
            if (!File.Exists(path)) path = Directory.GetParent(resXmlConfig).FullName + "\\Config\\" + resXmlConfig;

            XmlTextReader reader = new XmlTextReader(Path.GetFullPath(path));
            xmlDocument = new XmlDocument();
            xmlDocument.Load(reader);
            reader.Close();
        }

        private static ResXmlConfig instance = null;
        public static ResXmlConfig Instance
        {
            get { return instance = instance ?? new ResXmlConfig(); }
        }

        #endregion

        private static XmlDocument xmlDocument;
        public XmlDocument XmlDocument
        {
            get { return xmlDocument; }
            set { xmlDocument = value; }
        }


        /// <summary>
        /// 缺省图片src路径
        /// </summary>
        /// <param name="Domain"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string DefaultImgSrc(string Domain, string Name)
        {
            try
            {
                return Domain + xmlDocument.SelectSingleNode("//Images/Image[@Name='" + Name + "']").Attributes["Path"].Value;
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 根据声音文件名称获取声音
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string GetVoiceSrcByName(string Domain, string Name)
        {
            try
            {
                return Domain + xmlDocument.SelectSingleNode("//Voices/Voice[@Name='" + Name + "']").Attributes["Path"].Value;
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}

