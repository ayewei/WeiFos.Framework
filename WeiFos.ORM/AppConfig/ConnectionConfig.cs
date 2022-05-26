using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Core.SettingModule;

namespace WeiFos.ORM.AppConfig
{
    /// <summary>
    /// 数据库链接配置对象
    /// @author yewei 2015-04-04
    /// </summary>
    public class ConnectionConfig
    {

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        protected internal static string GetConnection(ConnectionLink connection)
        {
            var connectionStrings = "";
            switch (connection)
            {
                case ConnectionLink.Link1:
                    connectionStrings = ConfigManage.AppSettings<string>("DataBase:ConnectionString");
                    break;

                case ConnectionLink.Link2:
                    connectionStrings = ConfigManage.AppSettings<string>("DataBase:ConnectionString1");
                    break;

                case ConnectionLink.Link3:
                    connectionStrings = ConfigManage.AppSettings<string>("DataBase:ConnectionString2");
                    break;

                default:
                    throw new Exception("不存在该链接");
            }

            bool is_open = ConfigManage.AppSettings<bool>("AppSettings:IsOpenEncrpConn"); 
            if (is_open) return StringHelper.GetDecryption(connectionStrings);

            return connectionStrings;
        }
    }
}
