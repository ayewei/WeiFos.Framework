using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Admin.Controllers;
using WeiFos.Core;
using WeiFos.Core.ExcelModule;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.OrderModule;
using WeiFos.Entity.SystemModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.Service;
using WeiFos.Service.DistributionModule;
using WeiFos.Service.LogsModule;
using WeiFos.Service.MessageModule;
using WeiFos.Service.OrderModule;
using WeiFos.Service.WeChatModule;
using WeiFos.WeChat.Models;

namespace WeiFos.Admin.Areas.OrderModule.Controllers
{
    /// <summary>
    /// 订单 控制器
    /// @author yewei 
    /// add by @date 2015-03-25
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.OrderModule)]
    public class OrderController : BaseController
    {


        #region  商品订单


        /// <summary>
        /// 商品订单管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult ProductOrderManage(SysUser user)
        {
            return View();
        }



        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult ProductOrderForm(SysUser user)
        {
            //订单
            ProductOrder order = ServiceIoc.Get<ProductOrderService>().GetById(bid);

            //订单发货地址
            OrderDelivery delivery = ServiceIoc.Get<OrderDeliveryService>().GetByOrderId(bid);
            ViewBag.delivery = delivery;

            //订单详情
            List<ProductOrderDetail> order_details = ServiceIoc.Get<ProductOrderDetailService>().GetListByOrderId(bid);
            if (order == null || order_details == null) throw new Exception("订单数据异常");

            //物流公司
            ViewBag.companys = ServiceIoc.Get<LogisticsCompanyService>().GetAll();

            //判断是否为跳转过来的链接
            ViewBag.isUrlReferrer = Request.Headers["Referer"].FirstOrDefault() == null ? false : true;

            ViewBag.order = order;
            ViewBag.order_details = order_details;

            return View();
        }



        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetProductOrders(int pageSize, int pageIndex, int status, string keyword, string date)
        {
            return ServiceIoc.Get<ProductOrderService>().GetOrders(pageSize, pageIndex, status, keyword, date);
        }



        /// <summary>
        /// 确认发货
        /// </summary>
        /// <param name="orderDelivery"></param>
        /// <returns></returns>
        public JsonResult SaveOrderSend(OrderDelivery address)
        {
            StateCode state = ServiceIoc.Get<ProductOrderService>().SaveOrderSend(address);
            return Json(GetResult(state));
        }



        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult CloseOrder(long id)
        {
            StateCode state = StateCode.State_200;
            try
            {
                ProductOrder order = ServiceIoc.Get<ProductOrderService>().GetById(id);

                if (order == null)
                    state = StateCode.State_1;
                else
                {
                    order.status = OrderStatus.Close;
                    order.updated_date = DateTime.Now;
                    ServiceIoc.Get<ProductOrderService>().Update(order);
                }
            }
            catch
            {
                state = StateCode.State_500;
            }
            return Json(GetResult(state));
        }



        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public JsonResult UpdateAmount(SysUser user, ProductOrder order)
        {
            StateCode code = ServiceIoc.Get<ProductOrderService>().UpdateAmount(user.id, order);
            return Json(GetResult(code));
        }



        /// <summary>
        /// 保存订单物流信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public JsonResult SaveOrderLogistic(SysUser user, OrderDelivery address)
        {
            StateCode state = StateCode.State_200;
            try
            {
                ProductOrder order = ServiceIoc.Get<ProductOrderService>().GetById(address.order_id);

                if (order == null)
                    state = StateCode.State_551;
                else
                {
                    var orderdelivery = ServiceIoc.Get<OrderDeliveryService>().GetByOrderId(address.order_id);
                    if (orderdelivery != null)
                        state = StateCode.State_555;
                    else
                    {
                        orderdelivery.province = address.province;
                        orderdelivery.city = address.city;
                        orderdelivery.area = address.area;
                        orderdelivery.address = address.address;
                        orderdelivery.postal_code = address.postal_code;
                        orderdelivery.mobile = address.mobile;
                        orderdelivery.tel = address.tel;
                        orderdelivery.contact = address.contact;
                        orderdelivery.tracking_no = address.tracking_no;
                        orderdelivery.logistic_company = address.logistic_company;
                        ServiceIoc.Get<OrderDeliveryService>().Update(orderdelivery);
                    }
                }
            }
            catch
            {
                state = StateCode.State_500;
            }
            return Json(GetResult(state));
        }


        /// <summary>
        /// 获取近三天订单
        /// </summary>
        /// <returns></returns>
        public ContentResult GetDefaultOrders()
        {
            DataTable dt = ServiceIoc.Get<ProductOrderService>().GetDefaultOrders();
            return ContentResult(StateCode.State_200, dt);
        }


        #endregion


        #region 退款业务模块



        /// <summary>
        /// 商品订单管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult OrderRefundManage(SysUser user)
        {
            return View();
        }



        /// <summary>
        /// 退款单详情
        /// </summary>
        /// <param name="user"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult OrderRefundForm(SysUser user, string no)
        {
            //退款单
            OrderRefund orderRefund = ServiceIoc.Get<OrderRefundService>().Get(no);

            //订单
            ProductOrder order = ServiceIoc.Get<ProductOrderService>().GetById(orderRefund.order_id);

            //订单发货地址
            OrderDelivery delivery = ServiceIoc.Get<OrderDeliveryService>().GetByOrderId(orderRefund.order_id);

            //订单详情
            List<ProductOrderDetail> order_details = ServiceIoc.Get<ProductOrderDetailService>().GetListByOrderId(orderRefund.order_id);
            if (order == null || order_details == null) throw new Exception("订单数据异常");

            //物流公司
            ViewBag.companys = ServiceIoc.Get<LogisticsCompanyService>().GetAll();

            //判断是否为跳转过来的链接
            ViewBag.isUrlReferrer = Request.Headers["Referer"].FirstOrDefault() == null ? false : true;

            //订单信息
            ViewBag.order = order;
            //订单详情
            ViewBag.order_details = order_details;
            //收件人信息
            ViewBag.delivery = delivery;
            //退款订单
            ViewBag.orderRefund = orderRefund;

            return View();
        }




        /// <summary>
        /// 获取退货单列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult GetOrderRefunds(int pageSize, int pageIndex, int status, string keyword, string date)
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetFromTables("v_odr_refund")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" }).AddOrderBy(new OrderBy("id", "desc"));
            //查询表达式
            MutilExpression me = new MutilExpression();

            //状态
            if (status != -1)
            {
                me.Add(new SingleExpression("status", LogicOper.EQ, status));
            }

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                me.Add(new SingleExpression("order_serial_no", LogicOper.LIKE, "", keyword));
                me.Add(new SingleExpression("contact", LogicOper.LIKE, " or ", keyword));
                me.Add(new SingleExpression("mobile", LogicOper.LIKE, " or ", keyword));
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
            }

            //日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<OrderRefundService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }




        /// <summary>
        /// 提交订单退款
        /// </summary>
        /// <param name="no"></param>
        /// <param name="refund_amount"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitRefundOrder(string no, decimal refund_amount)
        {
            try
            {
                //退款状态码
                StateCode state = StateCode.State_500;

                #region 订单基本状态判断

                //退款单
                OrderRefund orderRefund = ServiceIoc.Get<OrderRefundService>().Get(no);

                //订单
                ProductOrder order = ServiceIoc.Get<ProductOrderService>().GetById(orderRefund.order_id);

                //是否存在订单
                if (order == null) return Json(GetResult(StateCode.State_551));

                //订单是否未支付
                if (!order.is_pay) return Json(GetResult(StateCode.State_552));

                //退款金额是否大于实际支付金额
                if (refund_amount > order.actual_amount) return Json(GetResult(StateCode.State_554));

                //订单状态和退款单状态是可以退款的情况
                if (order.refund_status != 1 || orderRefund.status != 1) return Json(GetResult(StateCode.State_553));

                #endregion

                //返回信息
                string msg = string.Empty;

                bool PayStatus = ConfigManage.AppSettings<bool>("AppSettings:WXPayStatus"); 

                //支付宝支付
                if (PayMethod.AliPay == order.pay_method)
                {
                    //商户号ID
                    string app_id = ConfigManage.AppSettings<string>("AppSettings:ALIPAY_APP_ID");
                    //AliRefundPayBizContent bizContent = new AliRefundPayBizContent();
                    //bizContent.out_trade_no = orderRefund.order_serial_no;
                    //bizContent.refund_amount = PayStatus ? refund_amount : decimal.Parse("0.01");

                    ////string webRootPath = host.WebRootPath;
                    //string privateKeyPem = host.ContentRootPath + "Config\\alipay_cret\\rsa_private_key.pem";
                    //string publicKeyPem = host.ContentRootPath + "Config\\alipay_cret\\rsa_public_key.pem";

                    //IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, privateKeyPem, "json", "1.0", "RSA2", publicKeyPem, "utf-8", true);
                    //AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
                    //request.BizContent = JsonConvert.SerializeObject(bizContent);

                    //AlipayTradeRefundResponse response = client.Execute(request);
                    //if (!response.IsError)
                    //{
                    //    state = ServiceIoc.Get<OrderRefundService>().AliPayConfirmRefund(orderRefund.order_serial_no, order.id);
                    //}
                    //else { }

                }//微信app支付
                else if (PayMethod.WeChat_App == order.pay_method)
                {
                    RefundPay refundParam = new RefundPay();

                    //证书路径
                    DirectoryInfo Dir = Directory.GetParent(ConfigManage.AppSettings<string>("AppSettings:WeChat_App_CertPath"));
                    string certPath = Dir.Parent.Parent.FullName;

                    //证书密钥
                    //string certPwd = Settings.AppSettings("CertPwd");
                    //商户信息
                    WeChatMerchant merchant = ServiceIoc.Get<WeChatMerchantService>().Get(PayMethod.WeChat_JsApi);

                    refundParam.appid = merchant.app_id;
                    //商户号
                    refundParam.mch_id = merchant.mch_id;
                    //随机数
                    refundParam.nonce_str = StringHelper.CreateNoncestr(16);
                    //商户侧传给微信的订单号
                    refundParam.out_trade_no = orderRefund.order_serial_no;
                    //商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
                    refundParam.out_refund_no = orderRefund.refund_serial_no;
                    //订单总金额,单位为分
                    refundParam.total_fee = PayStatus ? (int)(refund_amount * 100) : 1;
                    //订单总金额,单位为分
                    refundParam.refund_fee = PayStatus ? (int)(refund_amount * 100) : 1;
                    //操作员帐号, 默认为商户号
                    refundParam.op_user_id = merchant.mch_id;

                    //退款
                    state = ServiceIoc.Get<OrderRefundService>().WeChatConfirmRefund(refundParam, order.id, certPath, merchant.mch_id, merchant.pay_key, out msg);

                }//微信公众号支付
                else if (PayMethod.WeChat_JsApi == order.pay_method || PayMethod.WeChat_Native == order.pay_method)
                {
                    RefundPay refundParam = new RefundPay();

                    //证书路径
                    DirectoryInfo Dir = Directory.GetParent(ConfigManage.AppSettings<string>("AppSettings:WeChat_App_CertPath"));
                    string certPath = Dir.Parent.Parent.FullName;

                    //证书密钥
                    //string certPwd = Settings.AppSettings("CertPwd");

                    WeChatMerchant merchant = ServiceIoc.Get<WeChatMerchantService>().Get(PayMethod.WeChat_JsApi);

                    //开放平台授权公众号信息
                    WeChatAccount weChatAccount = ServiceIoc.Get<WeChatAccountService>().Get();

                    refundParam.appid = weChatAccount.appid;
                    //商户号
                    refundParam.mch_id = merchant.mch_id;
                    //随机数
                    refundParam.nonce_str = StringHelper.CreateNoncestr(16);
                    //商户侧传给微信的订单号
                    refundParam.out_trade_no = orderRefund.order_serial_no;
                    //商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
                    refundParam.out_refund_no = orderRefund.refund_serial_no;
                    //订单总金额,单位为分
                    refundParam.total_fee = PayStatus ? (int)(refund_amount * 100) : 1;
                    //订单总金额,单位为分
                    refundParam.refund_fee = PayStatus ? (int)(refund_amount * 100) : 1;
                    //操作员帐号, 默认为商户号
                    refundParam.op_user_id = merchant.mch_id;

                    //退款
                    state = ServiceIoc.Get<OrderRefundService>().WeChatConfirmRefund(refundParam, order.id, certPath, merchant.mch_id, merchant.pay_key, out msg);
                }

                return Json(GetResult(state, msg));
            }
            catch (Exception ex)
            {
                ServiceIoc.Get<APILogsService>().Save("提交退款SubmitRefundOrder==>" + ex.ToString());
                return Json(GetResult(StateCode.State_500));
            }
        }


        #endregion



        #region  商品报表


        /// <summary>
        /// 商品报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductOrderReport()
        {
            return View();
        }



        /// <summary>
        /// 商品订单报表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult GetProductOrderReports(int pageSize, int pageIndex, string keyword, string date)
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetFromTables("tb_odr_order")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*", "(total_amount - cost_price - freight - coupon_amount - discount_amount) profit" }).AddOrderBy(new OrderBy("id", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("serial_no", LogicOper.LIKE, "", keyword));
            }

            //日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<ProductOrderService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 商品订单报表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public JsonResult GetTotalProductOrder(string keyword, string date)
        {
            DataTable dt = ServiceIoc.Get<ProductOrderService>().GetSum(keyword, date);
            return Json(GetResult(StateCode.State_200, JsonConvert.SerializeObject(dt)));
        }




        /// <summary>
        /// 商品销售明细
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult GetSaleDetailReports(int pageSize, int pageIndex, string keyword, string date)
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetFromTables("v_odr_order_detail")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*", "(actual_amount - cost_price) profit" }).AddOrderBy(new OrderBy("id", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                me.Add(new SingleExpression("product_name", LogicOper.LIKE, "", keyword));
                me.Add(new SingleExpression("sup_name", LogicOper.LIKE, " or ", keyword));
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
            }

            //日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<ProductOrderService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 商品销售明细小计报表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public JsonResult GetTotalSaleDetail(string keyword, string date)
        {
            DataTable dt = ServiceIoc.Get<ProductOrderService>().GetTotalSaleDetails(keyword, date);
            return Json(GetResult(StateCode.State_200, JsonConvert.SerializeObject(dt)));
        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="user"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public FileResult ProductOrdersExportExcel(SysUser user, string keyword, string date)
        {

            #region 查询条件

            //查询对象
            Criteria ct = new Criteria();
            ct.SetFromTables("tb_odr_order")
            .SetFields(new string[] { "*", "(total_amount - cost_price -freight -coupon_amount - discount_amount) profit" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                me.Add(new SingleExpression("serial_no", LogicOper.LIKE, "", keyword));
                me.Add(new SingleExpression("product_name", LogicOper.LIKE, " or ", keyword));
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
            }

            //日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //订单类型
            me.Add(new SingleExpression("type", LogicOper.EQ, 0));

            //设置查询条件
            ct.SetWhereExpression(me);


            #endregion

            //查询数据
            DataTable dt = ServiceIoc.Get<ProductOrderService>().Fill(ct);

            #region 处理列头
            var ndt = new DataTable("Datas");
            ndt.Columns.Add("定单号");
            ndt.Columns.Add("订单总额(元)");
            ndt.Columns.Add("总成本(元)");
            ndt.Columns.Add("会员优惠(元)");
            ndt.Columns.Add("优惠卷(元)");
            ndt.Columns.Add("运费(元)");
            ndt.Columns.Add("实付金额(元)");
            ndt.Columns.Add("利润(元)");
            ndt.Columns.Add("创建日期");
            #endregion

            #region 处理数据行

            decimal total_amount = 0, actual_amount = 0, cost_price = 0, discount_amount = 0, coupon_amount = 0, freight = 0, profit = 0;

            var rows = ndt.NewRow();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    var nr = ndt.NewRow();
                    nr["定单号"] = r["serial_no"].ToString();
                    nr["订单总额(元)"] = r["total_amount"].ToString();
                    nr["总成本(元)"] = r["cost_price"].ToString();
                    nr["会员优惠(元)"] = r["discount_amount"];
                    nr["优惠卷(元)"] = r["coupon_amount"].ToString();
                    nr["运费(元)"] = r["freight"].ToString();
                    nr["实付金额(元)"] = r["actual_amount"].ToString();
                    nr["利润(元)"] = r["profit"].ToString();
                    nr["创建日期"] = r["created_date"].ToString();

                    ndt.Rows.Add(nr);

                    total_amount += decimal.Parse(r["total_amount"].ToString());
                    actual_amount += decimal.Parse(r["actual_amount"].ToString());
                    coupon_amount += decimal.Parse(r["coupon_amount"].ToString());
                    cost_price += decimal.Parse(r["cost_price"].ToString());
                    discount_amount += decimal.Parse(r["discount_amount"].ToString());
                    freight += decimal.Parse(r["freight"].ToString());
                    profit += decimal.Parse(r["profit"].ToString());
                }

                //添加到表格
                ndt.Rows.Add(rows);

                #region 添加小计
                var nr1 = ndt.NewRow();
                nr1["定单号"] = "";
                nr1["订单总额(元)"] = total_amount.ToString("0.00");
                nr1["总成本(元)"] = cost_price.ToString("0.00");
                nr1["会员优惠(元)"] = discount_amount.ToString("0.00");
                nr1["优惠卷(元)"] = coupon_amount.ToString("0.00");
                nr1["运费(元)"] = freight.ToString("0.00");
                nr1["实付金额(元)"] = actual_amount.ToString("0.00");
                nr1["利润(元)"] = profit.ToString("0.00");
                nr1["创建日期"] = "";

                ndt.Rows.Add(nr1);
                #endregion
            }

            #endregion

            return File(ExcelRender.RenderToExcel(ndt), "application/vnd.ms-excel", "商品订单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="user"></param>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public FileResult SaleDetailExportExcel(SysUser user, string keyword, string date)
        {

            #region 查询条件

            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetFromTables("v_odr_order_detail")
            .SetFields(new string[] { "*", "(actual_amount - cost_price) profit" }).AddOrderBy(new OrderBy("id", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                me.Add(new SingleExpression("serial_no", LogicOper.LIKE, "", keyword));
                me.Add(new SingleExpression("product_name", LogicOper.LIKE, " or ", keyword));
                me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
            }

            //日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<ProductOrderService>().Fill(ct);

            #endregion 

            #region 导出Excel

            #region newTable
            var ndt = new DataTable("Datas");
            ndt.Columns.Add("订单号");
            ndt.Columns.Add("供应商名称");
            ndt.Columns.Add("商品名称");
            ndt.Columns.Add("单价(元)");
            ndt.Columns.Add("商品数量");
            ndt.Columns.Add("小计金额(元)");
            ndt.Columns.Add("成本价(元)");
            ndt.Columns.Add("利润(元)");
            ndt.Columns.Add("创建日期");
            #endregion

            #endregion

            var recharge = ndt.NewRow();
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal actual_amount = 0, cost_price = 0, num = 0, unit_price = 0, profit = 0;
                foreach (DataRow r in dt.Rows)
                {
                    var nr = ndt.NewRow();
                    nr["订单号"] = r["serial_no"].ToString();
                    nr["供应商名称"] = r["sup_name"].ToString();
                    nr["商品名称"] = r["product_name"].ToString();
                    nr["单价(元)"] = r["unit_price"];
                    nr["商品数量"] = r["count"].ToString();
                    nr["小计金额(元)"] = r["actual_amount"].ToString();
                    nr["成本价(元)"] = r["cost_price"].ToString();
                    nr["利润(元)"] = r["profit"].ToString();
                    nr["创建日期"] = r["created_date"].ToString();

                    ndt.Rows.Add(nr);

                    num += int.Parse(r["count"].ToString());
                    actual_amount += decimal.Parse(r["actual_amount"].ToString());
                    cost_price += decimal.Parse(r["cost_price"].ToString());
                    unit_price += decimal.Parse(r["unit_price"].ToString());
                    profit += decimal.Parse(r["profit"].ToString());
                }

                ndt.Rows.Add(ndt.NewRow());

                #region 添加小计
                var nr1 = ndt.NewRow();
                nr1["订单号"] = "";
                nr1["供应商名称"] = "";
                nr1["商品名称"] = "";
                nr1["单价(元)"] = "";
                nr1["商品数量"] = num.ToString();
                nr1["小计金额(元)"] = actual_amount.ToString("0.00");
                nr1["成本价(元)"] = cost_price.ToString("0.00");
                nr1["利润(元)"] = profit.ToString("0.00");
                nr1["创建日期"] = "";

                ndt.Rows.Add(nr1);
                #endregion
            }

            return File(ExcelRender.RenderToExcel(ndt), "application/vnd.ms-excel", "商品销售明细_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }


        #endregion


    }
}