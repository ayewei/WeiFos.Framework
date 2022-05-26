using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Ioc
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = false, AllowMultiple = true), Serializable]
    public class ServiceAttribute : Attribute
    {
        public string Name { set; get; }
    }
}
