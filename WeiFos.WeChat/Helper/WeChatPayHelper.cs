using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Core.XmlHelper;
using WeiFos.WeChat.Common;
using WeiFos.WeChat.MessageModule;
using WeiFos.WeChat.Models;
using WeiFos.WeChat.Models.WXPay;
using WeiFos.WeChat.WXAlgorithm;

namespace WeiFos.WeChat.Helper
{
    /// <summary>
    /// 微信支付接口帮助类
    /// @Author yewei 
    /// @Date 2015-11-02
    /// </summary>
    public class WeChatPayHelper
    {


        /// <summary>
        /// 统一支付
        /// </summary>
        /// <param name="unified_order"></param>
        /// <returns></returns>
        public static UnifiedPrePay UnifiedPrePay(UnifiedOrder unified_order)
        {
            string PrePayPackage = XmlConvertHelper.SerializeObject<UnifiedOrder>(unified_order);
            string result = HttpClientHelper.HttpPostAsyn("https://api.mch.weixin.qq.com/pay/unifiedorder", PrePayPackage);
            return XmlConvertHelper.DeserializeObject<UnifiedPrePay>(result);
        }


        /// <summary>
        /// 线下扫码支付
        /// </summary>
        /// <param name="unified_order"></param>
        /// <returns></returns>
        public static ScanCodeUnifiedOrder ScanCodePay(UnifiedOrder unified_order)
        {
            string PrePayPackage = XmlConvertHelper.SerializeObject<UnifiedOrder>(unified_order);
            string result = HttpClientHelper.HttpPostAsyn("https://api.mch.weixin.qq.com/pay/micropay", PrePayPackage);
            return XmlConvertHelper.DeserializeObject<ScanCodeUnifiedOrder>(result);
        }
         
        

        /// <summary>
        /// 创建扫码支付二维码链接
        /// </summary>
        /// <param name="product_id"></param>
        /// <returns></returns>
        public static string CreateQRCodeUrl(string appid, string mch_id, string product_id, string paykey)
        {
            CreateQRParam qrparam = new CreateQRParam();
            qrparam.appid = appid;
            qrparam.mch_id = mch_id;
            qrparam.product_id = product_id;
            qrparam.nonce_str = StringHelper.CreateNoncestr(16);
            qrparam.time_stamp = WXAlgorithmHelper.GetTimeStamp;
            qrparam.sign = WXAlgorithmHelper.GetSign<CreateQRParam>(qrparam, paykey);

            string url = "weixin://wxpay/bizpayurl?appid={0}&mch_id={0}&nonce_str={0}&product_id={0}&sign={0}&time_stamp={0}";
            return string.Format(url, appid, mch_id, qrparam.nonce_str, product_id, qrparam.sign, qrparam.time_stamp);
        }


        /// <summary>
        /// 微信网页端调起支付API
        /// </summary>
        /// <param name="payMessage"></param>
        /// <param name="PayKey"></param>
        /// <returns></returns>
        public static string GetWeChatPayJsApi(UnifiedPrePay payMessage, string PayKey)
        {
            //预付单请求成功
            if (payMessage != null && payMessage.ResultSuccess() && payMessage.ReturnSuccess() && !string.IsNullOrEmpty(payMessage.prepay_id))
            {
                string timestamp = WXAlgorithmHelper.GetTimeStamp;
                string nonce_str = StringHelper.CreateNoncestr(16);

                var paytmp = new
                {
                    appId = payMessage.appid,
                    timeStamp = timestamp,
                    nonceStr = nonce_str,
                    package = "prepay_id=" + payMessage.prepay_id,
                    signType = "MD5"
                };

                //微信签名
                string sign_pay = WXAlgorithmHelper.GetSign<dynamic>(paytmp, PayKey);
                var paymsg = new
                {
                    //appId = payMessage.appid,
                    timestamp = timestamp,
                    nonceStr = nonce_str,
                    package = "prepay_id=" + payMessage.prepay_id,
                    signType = "MD5",
                    paySign = sign_pay
                };

                var data = new
                {
                    State = 200,
                    Data = JsonConvert.SerializeObject(paymsg)
                };

                return JsonConvert.SerializeObject(data);
            }
            else
            {
                var data = new
                {
                    Msg = payMessage,
                    State = 500
                };
                return JsonConvert.SerializeObject(data);
            }
        }


        /// <summary>
        /// 微信网页端调起支付API
        /// </summary>
        /// <param name="payMessage"></param>
        /// <param name="PayKey"></param>
        /// <returns></returns>
        public static Tuple<int, dynamic> GetWeChatPayMini(UnifiedPrePay payMessage, string PayKey)
        {
            //预付单请求成功
            if (payMessage != null && payMessage.ResultSuccess() && payMessage.ReturnSuccess() && !string.IsNullOrEmpty(payMessage.prepay_id))
            {
                //加密方式
                string signType = "MD5";
                //时间戳从1970年1月1日00:00:00至今的秒数,即当前的时间
                string timeStamp = WXAlgorithmHelper.GetTimeStamp;
                //随机字符串，不长于32位
                string nonceStr = StringHelper.CreateNoncestr(16);
                //微信分配的小程序ID，服务商模式下应为当前调起支付小程序的appid，此处还存在子商户对应多个小程序的场景
                string appId = string.IsNullOrEmpty(payMessage.sub_appid) && string.IsNullOrEmpty(payMessage.sub_mch_id) ? payMessage.appid : payMessage.sub_appid;
                //统一下单接口返回的 prepay_id 参数值，提交格式如：prepay_id=*
                string package = "prepay_id=" + payMessage.prepay_id;

                //小程序支付包
                var paymsg = new
                {
                    appId,
                    timeStamp,
                    nonceStr,
                    package,
                    signType,
                    //微信签名
                    paySign = WXAlgorithmHelper.GetSign<dynamic>(new { appId, timeStamp, nonceStr, package, signType }, PayKey)
                };

                return new Tuple<int, dynamic>(200, paymsg);
            }
            else
            {
                return new Tuple<int, dynamic>(500, payMessage);
            }
        }


        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="pakkey"></param>
        /// <returns></returns>
        public static OrderQuery QueryOrder(OrderQueryParam queryParam, string pakkey)
        {
            if (string.IsNullOrEmpty(queryParam.out_trade_no)) throw new Exception("商户订单号为空");

            //随机数
            queryParam.nonce_str = StringHelper.CreateNoncestr(16);
            //签名
            queryParam.sign = WXAlgorithmHelper.GetSign<OrderQueryParam>(queryParam, pakkey);

            string postData = XmlConvertHelper.SerializeObject<OrderQueryParam>(queryParam);
            string responseData = HttpClientHelper.PostURL("https://api.mch.weixin.qq.com/pay/orderquery", postData);
            return XmlConvertHelper.DeserializeObject<OrderQuery>(responseData);
        }


        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="refundParam">申请退款接口对象参数</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <param name="paykey">商户支付秘钥</param>
        /// <returns></returns>
        public static RefundMessage CallRefund(RefundPay refundParam, string certPath, string certPwd, string paykey)
        {
            //退款消息
            RefundMessage message = new RefundMessage();

            try
            {
                //缺少订单号
                if (string.IsNullOrEmpty(refundParam.out_refund_no)) throw new Exception("缺少订单号！");
                //退款金额必须大于0
                if (refundParam.refund_fee <= 0) throw new Exception("退款金额必须大于0");
                //签名
                refundParam.sign = WXAlgorithmHelper.GetSign<RefundPay>(refundParam, paykey);
                //序列化数据
                string postData = XmlConvertHelper.SerializeObject<RefundPay>(refundParam);
                //请求退款
                string responseData = HttpClientHelper.PostURL("https://api.mch.weixin.qq.com/secapi/pay/refund", postData, certPath, certPwd);
                //回调信息
                return XmlConvertHelper.DeserializeObject<RefundMessage>(responseData);
            }
            catch (Exception ex)
            {
                message.return_msg = ex.Message;
                return message;
            }
        }


        /// <summary>
        /// 企业付款 重要提醒：
        /// 当返回错误码为“SYSTEMERROR”时，请不要更换商户订单号，
        /// 一定要使用原商户订单号重试，否则可能造成重复支付等资金风险
        /// </summary>
        /// <param name="entPay">企业付款接口对象参数</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <param name="paykey">商户支付秘钥</param>
        /// <returns></returns>
        public static EntPayMessage EntPayment(EntPay entPay, string certPath, string certPwd, string paykey)
        {
            //退款消息
            EntPayMessage message = new EntPayMessage();
            try
            {
                //缺少订单号
                if (string.IsNullOrEmpty(entPay.partner_trade_no)) throw new Exception("缺少订单号！");
                //退款金额必须大于0
                if (entPay.amount <= 0) throw new Exception("退款金额必须大于0");
                //签名
                entPay.sign = WXAlgorithmHelper.GetSign<EntPay>(entPay, paykey);
                //序列化数据
                string postData = XmlConvertHelper.SerializeObject<EntPay>(entPay);
                //请求退款
                string responseData = HttpClientHelper.PostURL("https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers", postData, certPath, certPwd);
                //回调信息
                return XmlConvertHelper.DeserializeObject<EntPayMessage>(responseData);
            }
            catch (Exception ex)
            {
                message.return_msg = ex.Message;
                return message;
            }
        }




    }
}
