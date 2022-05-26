using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.Core.RouteConfig
{
    /// <summary>
    /// 路由配置集合
    /// </summary>
    public class RouteConfigCollection : List<RouteItem>
    {  }

    /// <summary>
    /// 路由配置
    /// </summary>
    public class RouteItem
    {
        public string name { get; set; }
        public string routeTemplate { get; set; }
        public @default defaults { get; set; }
        public @constraint constraints { get; set; }
        public class @default
        {
            public string controller { get; set; }
            public string action { get; set; }
            public object id { get; set; }
            public object version { get; set; }
        }

        public class @constraint
        {
            public object id { get; set; }
        }
    }

}