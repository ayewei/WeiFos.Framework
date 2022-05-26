using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.CodeBuilder.Builder;
using WeiFos.CodeBuilder.Entity;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.CodeBuildModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.CodeBuildModule
{
    /// <summary>
    /// 服务器链接 Service
    /// @author yewei
    /// add by  @date 2018-10-28
    /// </summary>
    public class ProjectModuleService : BaseService<ProjectModule>
    {



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, ProjectModule entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert<ProjectModule>(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = user_id;
                        s.Update<ProjectModule>(entity);
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
        /// 根据模块获取
        /// </summary>
        /// <param name="link_id"></param>
        /// <returns></returns>
        public List<ProjectModule> GetListByPId(int pid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProjectModule>("where project_setting_id = @0", pid);
            }
        }



        /// <summary>
        /// 获取模块Domain命名空间
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public string[] GetProjectModule(int mid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string[] arr = new string[2];
                arr[0] = "Weifos";
                arr[1] = "Default";

                ProjectModule module = s.Get<ProjectModule>(mid);
                if (module != null)
                {
                    ProjectSetting projectSetting = s.Get<ProjectSetting>(module.project_setting_id);
                    if (projectSetting != null)
                    {
                        arr[0] = projectSetting.en_name;
                        arr[1] = module.en_name;
                    }
                }

                return arr;
            }
        }



        /// <summary>
        /// 获取模块Service命名空间
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public string GetServiceSpace(int mid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string name_space = "Weifos.Service";

                ProjectModule module = s.Get<ProjectModule>(mid);
                if (module != null)
                {
                    ProjectSetting projectSetting = s.Get<ProjectSetting>(module.project_setting_id);
                    if (projectSetting != null)
                    {
                        name_space = projectSetting.en_name + ".Service." + module.en_name;
                    }
                }

                return name_space;
            }
        }






    }
}
