using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.WeChatModule.WXOpen;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 公众号临时数据对象
    /// @author yewei 
    /// @date 2018-04-14
    /// </summary>
    public class WeChatAccountTmp : WXOpenAccount
    {

        /// <summary>
        /// 秘钥
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        /// 公众号AppId
        /// </summary>
        public string appid { get; set; }


    }
}
