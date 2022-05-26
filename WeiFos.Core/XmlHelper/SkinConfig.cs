using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

/// <summary>
/// 微商城配置实体类
/// @author 叶委    
/// @date 2014-05-25
/// </summary>
namespace WeiFos.Core.XmlHelper
{
    public class SkinConfig
    {
        #region 单例模式
        private static SkinConfig instance = new SkinConfig();

        public static SkinConfig Instance
        {
            get { return SkinConfig.instance; }
        }
        #endregion


        private static XmlDocument xmlDocument;
        public XmlDocument XmlDocument
        {
            get { return xmlDocument; }
            set { xmlDocument = value; }
        }

        private Dictionary<string, string> skins;
        public Dictionary<string, string> Skins
        {
            get { return skins; }
            set { skins = value; }
        }

        /// <summary>
        /// 初始化资源
        /// </summary>
        public void Initial()
        {
            XmlTextReader reader = new XmlTextReader(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName + @"Config\MallThemeConfig.xml");
            xmlDocument = new XmlDocument();
            xmlDocument.Load(reader);
            reader.Close();

            if (Skins == null)
            {
                skins = new Dictionary<string, string>();
                foreach (XmlNode xml in xmlDocument.SelectNodes("//Skins"))
                {
                    skins.Add(xml.Attributes["name"].Value, xml.Attributes["value"].Value);
                }
            }
        }

        /// <summary>
        /// 根据编号获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetNo(string value)
        {
            try
            {
                return  xmlDocument.SelectSingleNode("//ThemeConfig/Skins[@value='" + value + "']").Attributes["no"].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取当前主题
        /// </summary>
        /// <param name="aId"></param>
        /// <param name="bId"></param>
        /// <param name="biz_type"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSkins()
        {
            return skins;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetValueByKey(string key)
        {
            foreach (var item in Instance.Skins)
            {
                if (key.Equals(item.Key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
