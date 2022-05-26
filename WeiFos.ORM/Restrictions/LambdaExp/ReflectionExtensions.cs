// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Reflection;

namespace WeiFos.ORM.Restrictions.LambdaExp
{
    /// <summary>
    /// 
    /// </summary>
	public static class ReflectionExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
		public static object GetValue(this MemberInfo member, object instance)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Property:
					return ((PropertyInfo)member).GetValue(instance, null);
				case MemberTypes.Field:
					return ((FieldInfo)member).GetValue(instance);
				default:
					throw new InvalidOperationException();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
		public static void SetValue(this MemberInfo member, object instance, object value)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Property:
					var pi = (PropertyInfo)member;
					pi.SetValue(instance, value, null);
					break;
				case MemberTypes.Field:
					var fi = (FieldInfo)member;
					fi.SetValue(instance, value);
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
