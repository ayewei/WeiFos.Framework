using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信菜单按钮
    /// </summary>
    public class UrlButton : Button
    {
        public string type { get; set; }
        public string url { get; set; }
    }
}
