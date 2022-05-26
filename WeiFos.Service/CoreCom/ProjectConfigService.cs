using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core;
using WeiFos.Entity.CoreCom;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data;

namespace WeiFos.Service.CoreCom
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectConfigService : BaseService<ProjectConfig>
    {

        /// <summary>
        /// 名称是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int ExistName(long id, string name)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<ProjectConfig>("where id != @0 and name = @1", id, name);
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public StateCode Save(ProjectConfig project)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    project.pvalue = AlgorithmHelper.GetPMValue(project.pkey);
                    if (project.id == 0)
                    {
                        s.Insert<ProjectConfig>(project);
                    }
                    else
                    {
                        s.Update<ProjectConfig>(project);
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 根据Key获取项目
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ProjectConfig Get(string key)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ProjectConfig>("where pkey = @0", key);
            }
        }



        /// <summary>
        /// 设置是否可用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public StateCode SetEnable(long id, bool isEnable)
        { 
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_prj_config set is_check = @0  where id = @1 ", isEnable, id);
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            } 
        }



    }
}
