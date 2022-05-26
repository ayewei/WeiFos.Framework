using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;
using System.Reflection;
using WeiFos.ORM.Data.Const;

namespace WeiFos.ORM.Data.Config
{
    /// <summary>
    /// @author yewei 
    /// 实体对象和表的映射关系类
    /// </summary>
    public class EntityConfig
    {
        /// <summary>
        /// 实体对象类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 实体描叙Attribute，对应得表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体对象所有的属性关系
        /// </summary>
        public PropertyConfig[] PropertyConfigs { get; set; }

        /// <summary>
        /// 获取所有的ID属性
        /// </summary>
        public string[] IDFields
        {
            get
            {
                return PropertyConfigs.Where(s => s.IsPrimaryKey).Select(s => s.PropertyInfo.Name).ToArray();
            }
        }

        /// <summary>
        /// 获取所有的非主键属性
        /// </summary>
        public string[] Fields
        {
            get
            {
                return PropertyConfigs.Where(s => !s.IsPrimaryKey).Select(s => string.IsNullOrEmpty(s.FileName) ? s.PropertyInfo.Name : s.FileName).ToArray();
            }
        }

        /// <summary>
        /// 获取需要交互数据库的属性
        /// </summary>
        public PropertyConfig[] DBProps<T>(T entity)
        {
            return PropertyConfigs.Where(p => !p.UnMappingField).Where(p => !p.IsPrimaryKey && p.PropertyInfo.GetValue(entity, null) != null || (p.IsPrimaryKey && p.Generator == KeyGenerator.Manual)).ToArray();
        }

        /// <summary>
        /// 获取需要交互数据库的属性字段名
        /// </summary>
        public string[] DBFields(PropertyConfig[] pcs)
        {
            return pcs.Select(p => string.IsNullOrEmpty(p.FileName) ? p.PropertyInfo.Name : p.FileName).ToArray();
        }

        /// <summary>
        /// 解析类类型，反射分析类
        /// </summary>
        /// <param name="type">Type 类的类型</param>
        /// <returns></returns>
        public static EntityConfig Parse(Type type)
        {
            //创建记录类和表映射类的实例
            EntityConfig ec = new EntityConfig();
            //当前对象类型
            ec.EntityType = type;

            if (type.GetCustomAttributes(typeof(TableAttribute), false).Length > 0)
            {
                TableAttribute TAttribute = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false)[0];
                ec.TableName = TAttribute.Name;
            }
            else
            {
                ec.TableName = type.Name;
            }

            //获取所有属性 数据类型为 PropertyInfo
            PropertyInfo[] propertys = type.GetProperties();

            ec.PropertyConfigs = new PropertyConfig[propertys.Length];

            for (int i = 0; i < propertys.Length; i++)
            {
                //当前属性配置
                PropertyConfig propertyConfig = new PropertyConfig();
                propertyConfig.PropertyInfo = propertys[i];

                //该建是否是主键
                if (propertys[i].GetCustomAttributes(typeof(IDAttribute), false).Length > 0)
                {
                    //当为主键时
                    IDAttribute idAttrs = (IDAttribute)propertys[i].GetCustomAttributes(typeof(IDAttribute), false)[0];
                    propertyConfig.IsPrimaryKey = true;
                    propertyConfig.Generator = idAttrs.Generator;
                }
                else
                {
                    propertyConfig.IsPrimaryKey = false;
                }

                //该属性是否需要映射
                if (propertys[i].GetCustomAttributes(typeof(UnMappedAttribute), false).Length > 0)
                    propertyConfig.UnMappingField = true;

                //该属性是否需要映射
                if (propertys[i].GetCustomAttributes(typeof(FileNameAttribute), false).Length > 0)
                {
                    FileNameAttribute file = (FileNameAttribute)propertys[i].GetCustomAttributes(typeof(FileNameAttribute), false)[0];
                    propertyConfig.FileName = file.Name;
                }
                else
                {
                    propertyConfig.FileName = propertys[i].Name;
                }

                ec.PropertyConfigs[i] = propertyConfig;
            }

            return ec;
        }


    }
}
