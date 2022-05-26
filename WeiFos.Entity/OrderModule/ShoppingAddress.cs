using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 收货地址 实体类
    /// @author yewei 
    /// @date 2014-03-24
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_user_shipping_address")]
    public class ShoppingAddress
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// open_id
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

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
        /// 是否是默认收货地址
        /// </summary>
        public bool is_default { get; set; }

        /// <summary>
        /// 获取收货地址
        /// </summary>
        /// <returns></returns>
        public string GetAddressDetails()
        {
            return string.Format("{0}{1}{2}{3}<br/>{4} {5} {6}", province, city, area, address, contact, mobile, tel);
        }

        /// <summary>
        /// 获取收货地址
        /// </summary>
        /// <returns></returns>
        public string GetAddress()
        {
            return string.Format("{0}{1}{2}{3}", province, city, area, address);
        }

    }
}
