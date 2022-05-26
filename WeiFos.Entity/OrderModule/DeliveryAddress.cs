using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 订单收货地址实体对象
    /// @author yewei 
    /// @date 2014-04-17
    /// </summary>
    [Serializable]
    [Table(Name = "tb_odr_delyaddress")]
    public class DeliveryAddress 
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 定单ID
        /// </summary>
        public int order_id { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string contact { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string postal_code { get; set; }

        /// <summary>
        /// 获取完整地址信息
        /// </summary>
        /// <returns></returns>
        public string GetAddress()
        {
            return province + city + area + address;
        }
    }
}
