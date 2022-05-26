using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WeiFos.SDK.Attributes;

namespace WeiFos.SDK.APIEntity
{

    /// <summary>
    /// @author yewei 
    /// 状态码注解说明帮助类
    /// @date 2022-04-29
    /// </summary>
    public class RspCodeHelper 
    {

        #region 单列模式  

        //使用System.Lazy<T> type来实现完全懒汉式
        private static readonly Lazy<RspCodeHelper> lazy = new Lazy<RspCodeHelper>(() => new RspCodeHelper());
        public static RspCodeHelper Instance { get { return lazy.Value; } }
        private RspCodeHelper() { }

        #endregion


        Dictionary<int, string> dicts = new Dictionary<int, string>();


        /// <summary>
        /// 获取状态码描叙
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetCodeDesc<T>(int code) where T : RspCode
        {
            //存在则返回
            foreach (var kvp in dicts) if (kvp.Key == code) return kvp.Value;

            //状态码信息
            string desc = null;
            //当前类类型
            Type t_code = typeof(T);
            //当前字段集合
            List<FieldInfo> fields = t_code.GetRuntimeFields().ToList();
            //父类字段集合
            var parents = t_code.BaseType.GetRuntimeFields().ToList();
            //字段汇总
            foreach (var item in parents) fields.Add(item);
            //注解实体属性集合
            foreach (var item in fields)
            {
                int scode = int.Parse(item.GetValue(item).ToString());
                if (scode == code)
                {
                    //自定义属性集合
                    var attr = (StateCodeAttribute)item.GetCustomAttributes(typeof(StateCodeAttribute), false)[0];
                    desc = attr.Describe;
                    break;
                }
            }

            //集合内部是否存在
            if (desc != null && !dicts.ContainsKey(code)) dicts.Add(code, desc);

            //返回状态码
            return desc;
        }



    }
}
