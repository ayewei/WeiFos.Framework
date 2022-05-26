using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.GaoDeng.APIModule
{
    /// <summary>
    /// 获取授权链接
    /// </summary>
    public class GDAuthUrl
    {
        /// <summary>
        /// 微信公众号appid
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 订单金额（以元为单位,最多保留两位小数）
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 订单金额（以元为单位,最多保留两位小数）
        /// </summary>
        public string money { get; set; }

        /// <summary>
        /// 时间戳(为10位时间戳)
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// [app, web, wxa]开票来源，app：app开票，web：微信h5开票，wxa：小程序开发票
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 授权成功后跳转页面地址
        /// </summary>
        public string redirect_url { get; set; }

        /// <summary>
        /// [0,1,2]授权类型，0：开票授权，1：填写字段开票授权，2：领票授权
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 授权完成回调地址
        /// </summary>
        public string callback_url { get; set; }


    }
}
