using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Common;
using WeiFos.Entity.SystemModule;
using WeiFos.Core;
using WeiFos.ORM.Data;
using WeiFos.Entity.OrderModule;
using WeiFos.WeChat.Models;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.MessageModule;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.MessageModule
{


    public class OrderRefundService : BaseService<OrderRefund>
    {

        /// <summary>
        /// 根据订单号获取退订订单
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public OrderRefund Get(long order_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<OrderRefund>("where order_id = @0 ", order_id);
            }
        }


        /// <summary>
        /// 根据订单号获取退订订单
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public OrderRefund Get(string order_no)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<OrderRefund>(" where refund_serial_no = @0 ", order_no);
            }
        }


        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="serial_no"></param>
        /// <returns></returns>
        public StateCode ApplyRefund(long user_id, string serial_no)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    ProductOrder order = s.Get<ProductOrder>("where serial_no = @0", serial_no);
                    if (order == null) return StateCode.State_551;

                    //是否支付
                    if (!order.is_pay) return StateCode.State_552;

                    //退款订单
                    OrderRefund refund = new OrderRefund();

                    //退款单
                    refund.order_id = order.id;
                    refund.created_user_id = user_id;
                    refund.status = OrderRefundStatus.Apply;
                    refund.order_serial_no = order.serial_no;
                    refund.refund_serial_no = AlgorithmHelper.CreateNo19("T");
                    //原订单金额
                    refund.order_actual_amount = order.actual_amount;
                    //退款总金额
                    refund.refund_total_amount = order.total_amount;
                    refund.created_date = DateTime.Now;
                    s.Insert(refund);

                    //修改订单状态
                    s.ExcuteUpdate("update tb_odr_order set refund_status = @0 where serial_no = @1 ", OrderRefundStatus.Apply, serial_no);

                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 拒绝退款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account_id"></param>
        public int Cancel(long id, string remarks, SysUser sysUser)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    OrderRefund refund = s.Get<OrderRefund>(@" where id = @0 ", id);
                    refund.status = OrderRefundStatus.NoPass;
                    refund.remarks = remarks;
                    refund.updated_user_id = sysUser.id;
                    refund.updated_date = DateTime.Now;
                    s.Update<OrderRefund>(refund);
                    return (int)StateCode.State_200;
                }
                catch
                {
                    return (int)StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 确认退款
        /// </summary>
        /// <param name="refundParam"></param>
        /// <param name="certPath"></param>
        /// <param name="certPwd"></param>
        /// <param name="paykey"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        public StateCode WeChatConfirmRefund(RefundPay refundParam, long order_id, string certPath, string certPwd, string paykey, out string return_msg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return_msg = string.Empty;
                try
                {
                    //回调
                    RefundMessage rmessage = WeChatPayHelper.CallRefund(refundParam, certPath, certPwd, paykey);
                    if (rmessage.return_code.Equals("FAIL", StringComparison.OrdinalIgnoreCase))
                    {
                        return_msg = rmessage.return_msg;
                        return StateCode.State_500;
                    }
                    else if (rmessage.result_code.Equals("FAIL", StringComparison.OrdinalIgnoreCase))
                    {
                        return_msg = rmessage.err_code + " " + rmessage.err_code_des;
                        return StateCode.State_500;
                    }

                    //开启事务
                    s.StartTransaction();

                    //更新订单状态
                    s.ExcuteUpdate("update tb_odr_order set refund_status = @0,status = @1 where id = @2 ", OrderRefundStatus.Pass, OrderStatus.Close, order_id);

                    //更新退款单状态
                    s.ExcuteUpdate("update tb_odr_refund set status = @0 where order_serial_no = @1 ", OrderRefundStatus.Pass, refundParam.out_trade_no);

                    //提交事务
                    s.Commit();

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    return_msg = ex.ToString();
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 退款状态修改
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public StateCode AliPayConfirmRefund(string refund_order_no, long order_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //开启事务
                    s.StartTransaction();

                    //更新订单状态
                    s.ExcuteUpdate("update tb_odr_order set refund_status = @0,status = @1 where id = @2 ", OrderRefundStatus.Pass, OrderStatus.Close, order_id);

                    //更新退款单状态
                    s.ExcuteUpdate("update tb_odr_refund set status = @0 where order_serial_no = @1 ", OrderRefundStatus.Pass, refund_order_no);

                    //提交事务
                    s.Commit();

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }






    }
}
