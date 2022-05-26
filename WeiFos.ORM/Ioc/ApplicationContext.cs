using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WeiFos.ORM.Ioc
{
    public class ApplicationContext : IApplicationContext
    {
        /// <summary>
        /// 存放系统所有的管理对象
        /// </summary>
        private Dictionary<string, object> singletonObjects = new Dictionary<string, object>();


        public ApplicationContext()
        {

        }

        private void initial()
        {
            Assembly assembly = Assembly.LoadFile("");

            assembly.GetTypes();
        }

    }
}
