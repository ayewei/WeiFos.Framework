using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ReplyModule
{
    /// <summary>
    /// Lbs实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_rpy_lbs")]
    public class LbsReply : BaseClass
    {
        #region Model
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 会员账户ID
        /// </summary>
        public int account_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string intro { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 详细页显示封面图片
        /// </summary>
        public bool showcover { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 地图标识地址
        /// </summary>
        public string lbsaddress { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double lng { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool is_enable { get; set; }

        #endregion Model

    }
}
