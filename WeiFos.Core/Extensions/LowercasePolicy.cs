using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace WeiFos.Core.Extensions
{
    /// <summary>
    /// 返回对象全小写
    /// </summary>
    public class LowercasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }

}
