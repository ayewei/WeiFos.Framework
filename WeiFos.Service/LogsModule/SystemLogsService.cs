using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.LogsModule;
using WeiFos.ORM.Data;


namespace WeiFos.Service.LogsModule
{
    /// <summary>
    /// 系统日志 Service 
    /// @author yewei 
    /// @date 2014-04-16
    /// </summary>
    public class SystemLogsService : BaseService<SystemLogs>
    {



        /// <summary>
        /// 清空日志
        /// </summary>
        /// <returns></returns>
        public StateCode Clear()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("delete tb_logs_sys_user_op");
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="type">0：操作日志，1：异常日志</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public StateCode Save(int type, string content)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    SystemLogs log = new SystemLogs();
                    log.type = type;
                    log.created_date = DateTime.Now;
                    log.content = content;
                    s.Insert<SystemLogs>(log);
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
            return StateCode.State_200;
        }



    }
}
