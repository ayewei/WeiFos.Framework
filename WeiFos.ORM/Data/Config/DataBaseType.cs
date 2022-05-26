using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.ORM.Data.Config
{
 
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：数据库类型 
    /// 创建人：叶委
    /// 创建日期：2013.03.18
    /// </summary>
    public class DataBaseType
    {
        /// <summary>
        /// Sqlite数据库
        /// </summary>
        public const string Sqlite = "Sqlite";

        /// <summary>
        /// MySql数据库
        /// </summary>
        public const string MySql = "MySql";

        /// <summary>
        /// Oracle数据库
        /// </summary>
        public const string Oracle = "Oracle";

        /// <summary>
        /// SqlServer数据库
        /// </summary>
        public const string SqlServer = "SqlServer";

    }
}
