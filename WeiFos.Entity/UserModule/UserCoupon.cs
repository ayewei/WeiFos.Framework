using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.UserModule
{
    /// <summary>
    /// 用户优惠券
    /// @author yewei 
    /// @date 2016-08-16
    /// </summary>
    [Serializable]
    [Table(Name = "tb_user_aty_coupon")]
    public class UserCoupon
    {

        /// <summary>
        /// 用户ID 自增
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        public long activity_id { get; set; }

        /// <summary>
        /// 优惠劵ID
        /// </summary>
        public long coupon_id { get; set; }

        /// <summary>
        /// 优惠卷编号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 来源类型0注册赠送(新人卷)，1活动赠送
        /// </summary>
        public int source_type { get; set; }

        /// <summary>
        /// 卷类型（0：商品优惠卷:1：课程优惠卷）
        /// </summary>
        public int coupon_type { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool is_used { get; set; }

        /// <summary>
        /// 金额大小
        /// </summary>
        public decimal coupon_amount { get; set; }

        /// <summary>
        /// 订单满额使用条件
        /// </summary>
        public decimal full_amount { set; get; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// 有效开始日期
        /// </summary>
        public DateTime expiry_sdate { get; set; }

        /// <summary>
        /// 有效结束日期
        /// </summary>
        public DateTime expiry_edate { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string remark { get; set; }


    }
}
