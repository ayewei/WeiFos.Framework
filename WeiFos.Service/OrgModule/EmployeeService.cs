using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.OrgModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.OrgModule
{

    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-01-03 12:54:19
    /// 描 述：员工业务逻辑
    /// </summary>
    public class EmployeeService : BaseService<Employee>
    {


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, Employee entity, string imgmsg = null)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        s.Insert(entity);
                    }
                    else
                    {
                        s.Update(entity);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.SysUser, entity.id);
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", entity.id, ImgType.SysUser, imgmsg);
                    }

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.Insert(new SystemLogs() { content = ex.ToString(), created_date = DateTime.Now, type = 1 });
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 根据用户ID获取
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public Employee GetByUserId(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<Employee>("where sys_user_id = @0", user_id);
            }
        }



        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="k"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Employee> TopSearchNo(string k, int top = 10)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.GetTop<Employee>(top, "where name like '%'+@0+'%'", k);
            }
        }


        /// <summary>
        /// 是否存在编号
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public int ExistUserNo(string no, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<Employee>("where no = @0 and id != @1", no, id);
            }
        }




    }
}
