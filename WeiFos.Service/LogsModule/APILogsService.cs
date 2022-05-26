using WeiFos.Entity.LogsModule;
using System;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data;

namespace WeiFos.Service.LogsModule
{
    /// <summary>
    /// API日志 Service 
    /// @author yewei 
    /// @date 2015-11-15
    /// </summary>
    public class APILogsService : BaseService<APILogs>
    {


        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public StateCode Save(string content)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    APILogs log = new APILogs();
                    log.created_date = DateTime.Now;
                    log.content = content;
                    s.Insert<APILogs>(log);
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
            return StateCode.State_200;
        }




        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public StateCode SaveError(string content)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    APILogs log = new APILogs();
                    log.type = 1;
                    log.created_date = DateTime.Now;
                    log.content = content;
                    s.Insert<APILogs>(log);
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
            return StateCode.State_200;
        }




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
                    s.ExcuteUpdate("delete tb_logs_api");
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }







    }

}
