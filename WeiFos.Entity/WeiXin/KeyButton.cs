using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信菜单按钮
    /// </summary>
    public class KeyButton : Button
    {
        public string type { get; set; }
        public string key { get; set; }
    }
}
