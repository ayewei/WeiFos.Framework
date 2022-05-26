using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.OrgEntity
{
    /// <summary>
    /// 企业微信部门信息 
    /// @author yewei 
    /// @date 2019-03-16
    /// </summary>
    [Serializable]
    public class WeChatDept : WXCodeError
    {

        /// <summary>
        /// 部门ID 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 父亲部门id。根部门为1
        /// </summary>
        public int parentid { get; set; }

        /// <summary>
        /// 在父部门中的次序值。order值大的排序靠前。值范围是[0, 2^32)
        /// </summary>
        public int order { get; set; }

    }
}
