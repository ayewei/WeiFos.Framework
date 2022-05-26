using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 订单发货人信息 
    /// @author yewei 
    /// @date 2014-04-17
    /// </summary>
    [Table(Name = "tb_odr_consignor")]
    public class OrderConsignor : BaseClass
    {
       
        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 发货点
        /// </summary>
        public string shipping_point { get; set; }

        /// <summary>
        /// 发货人姓名
        /// </summary>
        public string shipper_name { get; set; }

        /// <summary>
        /// 是否是默认发货地址
        /// </summary>
        public bool is_default { get; set; }

        /// <summary>
        /// 发货地区 省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 发货地区 市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 发货地区 县
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string postal_code { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remarks { get; set; }


        #endregion Model


    }
}
