using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信菜单及子菜单按钮
    /// </summary>
    public class ComplexButton : Button
    {
        public List<Button> sub_button { get; set; }
    }
}
