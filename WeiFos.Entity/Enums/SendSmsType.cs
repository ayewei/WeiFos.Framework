using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core.EnumHelper;

namespace WeiFos.Entity.Enums
{
    /// <summary>
    /// 短信业务类型
    /// @author yewei 
    /// @date 2015-05-17
    /// </summary>
    public enum SendSmsType
    {

        /// <summary>
        /// 会员注册
        /// </summary>
        [EnumAttribute("会员注册")]
        Register = 1,


        /// <summary>
        /// 绑定新手机
        /// </summary>
        [EnumAttribute("绑定新手机")]
        BindNewMobile = 5,


        /// <summary>
        /// 找回密码
        /// </summary>
        [EnumAttribute("找回密码")]
        ForgetPsw = 10


    }
}