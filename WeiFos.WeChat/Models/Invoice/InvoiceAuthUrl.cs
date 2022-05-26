using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.Invoice
{
    /// <summary>
    /// 获取授权页链接实体
    /// @Author yewei 
    /// @Date 2019-05-15
    /// </summary>
    public class InvoiceAuthUrl
    {

        /// <summary>
        /// 开票平台在微信的标识号，商户需要找开票平台提供
        /// </summary>
        public string s_pappid { get; set; }

        /// <summary>
        /// 订单id，在商户内单笔开票请求的唯一识别号，
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 订单金额，以分为单位
        /// </summary>
        public string money { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 开票来源，app：app开票，web：微信h5开票，wxa：小程序开发票，wap：普通网页开票
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 授权成功后跳转页面。本字段只有在source为H5的时候需要填写，
        /// 引导用户在微信中进行下一步流程。app开票因为从外部app拉起微信授权页，
        /// 授权完成后自动回到原来的app，故无需填写。
        /// </summary>
        public string redirect_url { get; set; }

        /// <summary>
        /// 从上一环节中获取
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 授权类型，0：开票授权，1：填写字段开票授权，2：领票授权
        /// </summary>
        public string type { get; set; }



    }
}
