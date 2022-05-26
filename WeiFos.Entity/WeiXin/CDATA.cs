using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 支持<![CDATA[]]> 模块
    /// @author yewei 
    /// @date 2013-11-05 
    /// </summary>
    public class CDATA : IXmlSerializable
    {

        private string m_Value;

        public CDATA()
        {
        }

        public CDATA(string p_Value)
        {
            m_Value = p_Value;
        }

        public string Value
        {
            get
            {
                return m_Value;
            }
        }

        public void ReadXml(XmlReader reader)
        {
            m_Value = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteCData(m_Value);
        }

        public XmlSchema GetSchema()
        {
            return (null);
        }

        public override string ToString()
        {
            return m_Value;
        }

        public static implicit operator string(CDATA element)
        {
            return (element == null) ? null : element.m_Value;
        }

        public static implicit operator CDATA(string text)
        {
            return new CDATA(text);
        }

    }


}
