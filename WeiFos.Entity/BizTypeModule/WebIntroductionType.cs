using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.BizTypeModule
{
    /// <summary>
    /// 企业信息配置实体类
    /// @author yewei 
    /// @date 2013-09-11
    /// </summary>
    public class WebIntroductionType
    {

        /// <summary>
        /// 关于我们
        /// </summary>
        public const string AboutUs = "AboutUs";

        /// <summary>
        /// 联系我们
        /// </summary>
        public const string Contact = "Contact";

        /// <summary>
        /// 服务流程
        /// </summary>
        public const string Applc = "Applc";
          
        /// <summary>
        /// 问答
        /// </summary>
        public const string AntQa = "AntQa";

        /// <summary>
        /// 微狐信息科技邮箱发送模板
        /// </summary>
        public const string EmailTmp = "EmailTmp";


        public static Dictionary<string, string> intList = new Dictionary<string, string>()
        {
            {WebIntroductionType.Contact,"联系我们"},
            {WebIntroductionType.AboutUs,"关于我们"},
            {WebIntroductionType.Applc,"服务流程"}, 
            {WebIntroductionType.AntQa,"蚂蚁学院-蚂蚁问答" },
            {WebIntroductionType.EmailTmp,"微狐信息科技邮箱发送模板" }
        };


        public static string GetValueBykey(string key)
        {
            foreach (var item in intList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "暂无";
        }


    }


}