using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.OrgEntity
{
    /// <summary>
    /// 微信 部门集合
    /// 通过接口获取用户数据临时对象
    /// @author yewei 
    /// @date 2014-12-27
    /// </summary>
    [Serializable]
    public class WeChatEmployeeResult : WXCodeError
    {

        /// <summary>
        /// 员工结果集合
        /// </summary>
        public List<WeChatEmployee> userlist { get; set; }

    }
}
