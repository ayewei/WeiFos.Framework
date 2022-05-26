using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 操作日志Service
    /// @author yewei 
    /// @date 2013-09-22
    /// </summary>
    public class SysOperatingLogsService : BaseService<SysOperatingLogs>
    {

        /// <summary>
        /// 保存操作日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sysuserId"></param>
        public void Add(string content, int sysuserId)
        {
            SysOperatingLogs logs = new SysOperatingLogs();
            //操作日志内容
            logs.operating_content = content;
            //操作时间
            logs.operating_time = DateTime.Now;
            //操作用户
            logs.sysuser_id = sysuserId;

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.Insert<SysOperatingLogs>(logs);
            }
        }

        /// <summary>
        /// 清除前30天以外的操作日志
        /// </summary>
        public void ClearOperatingLogs()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前时间
                DateTime currentDate = DateTime.Now;

                //前30天
                DateTime ago = currentDate.AddDays(-30);

                s.ExcuteUpdate("delete from tb_sys_operatinglogs where operating_time <= @0 ", ago);
            }
        }

    }

}
