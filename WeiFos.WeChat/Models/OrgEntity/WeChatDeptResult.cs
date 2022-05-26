using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.OrgEntity
{
    /// <summary>
    /// 获取微信部门结果集合
    /// @author yewei 
    /// @date 2019-03-16
    /// </summary>
    [Serializable]
    public class WeChatDeptResult : WXCodeError
    {

        /// <summary>
        /// 部门ID 
        /// </summary>
        public List<WeChatDept> department { get; set; }
         
    }
}
