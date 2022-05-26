using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.BizTypeModule
{
    /// <summary>
    /// 数据库类型 实体类
    /// @author yewei 
    /// @date 2015-03-16
    /// </summary>
    public class DBType
    {


        /// <summary>
        /// 1：mysql
        /// </summary>
        public const int MySql = 1;

        /// <summary>
        /// 5：sql server
        /// </summary>
        public const int SqlServer = 5;

        /// <summary>
        /// 10：oracle
        /// </summary>
        public const int Oracle = 10;


        /// <summary>
        /// 集合
        /// </summary>
        public static Dictionary<int, string> DBList = new Dictionary<int, string>() {
            { DBType.MySql,"MySql"},
            { DBType.SqlServer,"SqlServer"},
            { DBType.Oracle,"Oracle"},
        };


        public static string Get(int key)
        {
            foreach (var item in DBList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "暂无";
        }



    }
}
