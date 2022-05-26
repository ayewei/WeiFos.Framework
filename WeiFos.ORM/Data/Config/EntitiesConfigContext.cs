using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Config
{
    /// <summary>
    /// @author yewei 
    /// 实体类与表映射关系类
    /// 将所有实体对象与表映射关系保存在内存中
    /// </summary>
    public class EntitiesConfigContext
    {
        /// <summary>
        /// 所有实体类相关配置
        /// </summary>
        private List<EntityConfig> entitiesConfigs = new List<EntityConfig>();

        /// <summary>
        /// 根据类型名称，查找实体对象的配置信息
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected EntityConfig GetEntityConfig(string typeName)
        {
            //return entitiesConfigs.Where(e => e.EntityType.FullName == typeName).SingleOrDefault();//type.FullName 取对应类的命名空间+类名

            lock (entitiesConfigs)
            {
                foreach (EntityConfig en in entitiesConfigs)
                {
                    if (en.EntityType.FullName.Equals(typeName))
                    {
                        return en;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 根据类类型找到实体和表映射关系
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public EntityConfig this[Type type]
        {
            get
            {
                EntityConfig econfig = GetEntityConfig(type.FullName);//type.FullName 取对应类的命名空间+类名

                if (econfig == null)
                {
                    econfig = EntityConfig.Parse(type);

                    entitiesConfigs.Add(econfig);
                }
                return econfig;
            }
        }


    }
}
