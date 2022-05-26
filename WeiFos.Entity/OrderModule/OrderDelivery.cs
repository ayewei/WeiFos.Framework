using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 订单发货信息 实体对象
    /// @author yewei 
    /// @date 2014-04-17
    /// </summary>
    [Table(Name = "tb_odr_delivery")]
    public class OrderDelivery
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long order_id { get; set; }

        /// <summary>
        /// 1：订单、2退货单
        /// </summary>
        public int order_type { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string contact { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string postal_code { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        public string logistic_company { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string tracking_no { get; set; }

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
