using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text; 
using WeiFos.Entity.CodeBuildModule;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data;

namespace WeiFos.Service.CodeBuildModule
{
    /// <summary>
    /// 数据库服务器链接 Service
    /// @author yewei
    /// add by  @date 2018-10-28
    /// </summary>
    public class ProjectSettingService : BaseService<ProjectSetting>
    {



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, ProjectSetting entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert<ProjectSetting>(entity);

                        if (entity.modules != null)
                        {
                            foreach (var c in entity.modules)
                            {
                                c.project_setting_id = entity.id;
                                c.created_date = DateTime.Now;
                                c.created_user_id = user_id;
                                s.Insert(c);
                            }
                        }
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = user_id;
                        s.Update(entity);
                    }
                     
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode Del(int id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //删除项目
                    s.Delete<ProjectSetting>(id);

                    //删除项目模块
                    s.ExcuteUpdate("delete tb_code_project_module where project_setting_id = @0", id);

                    s.Commit();

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="link_id"></param>
        /// <returns></returns>
        public List<ProjectSetting> GetByLinkId(int link_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProjectSetting>("where link_id = @0", link_id);
            }
        }





    }
}
