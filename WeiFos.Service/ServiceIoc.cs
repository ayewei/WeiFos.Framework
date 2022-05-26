using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace WeiFos.Service
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// 描 述：定义缓存接口
    /// </summary>
    public class ServiceIoc
    {
        /// <summary>
        /// ioc容器
        /// </summary>
        public IUnityContainer container;

        #region 单列模式  

        /*私有构造器，不能该类外部new对象*/
        private ServiceIoc()
        {
            if (instance == null || instance.container == null)
            {
                //UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                //container = new UnityContainer();
                //section.Configure(container, "ServiceContainer"); 
                container = new UnityContainer(); 
            }
        }

        private static ServiceIoc instance = null;
        private static ServiceIoc Instance
        {
            get { return instance = instance ?? new ServiceIoc(); }
        }

        #endregion

        public static T Get<T>() where T : new()
        {
            return Instance.container.Resolve<T>();
        }

    }
}
