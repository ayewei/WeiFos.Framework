using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WeiFos.ORM.Ioc
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = false, AllowMultiple = true), Serializable]
    public class ImplementByAttribute : Attribute
    {
        public string Name { set; get; }
    }
}
