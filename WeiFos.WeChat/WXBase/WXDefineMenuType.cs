using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core.EnumHelper;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信自定义菜单响应动作类型 
    /// </summary>
    public enum WXDefineMenuType
    {
        /// <summary>
        /// 点击推事件
        /// </summary>
        [EnumAttribute("点击推事件")]
        click,

        /// <summary>
        /// 跳转URL
        /// </summary>
        [EnumAttribute("跳转URL")]
        view,

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        [EnumAttribute("扫码推事件且弹出“消息接收中”提示框")]
        scancode_waitmsg,

        /// <summary>
        /// 扫码推事件
        /// </summary>
        [EnumAttribute("扫码推事件")]
        scancode_push,
    }
}
