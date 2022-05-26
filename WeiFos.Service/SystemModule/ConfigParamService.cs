using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 系统参数配置Service
    /// @author yewei 
    /// @date 2013-04-27
    /// </summary>
    public class ConfigParamService : BaseService<ConfigParam>
    {

        /// <summary>
        /// 保存系统参数
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, ConfigParam entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert(entity);
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
        /// 根据key 获取记录 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConfigParam Get(string key)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ConfigParam>("where config_key = @0", key);
            }
        }

         

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="is_enable"></param>
        public StateCode SetEnable(long id, bool is_enable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_sys_config_param set is_enable = @0 where id = @1", is_enable, id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 是否存在配置KEY
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ExistConfigParamKey(string key, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<ConfigParam>("where config_key = @0 and id != @1", key, id);
            }
        }  


    }
}
