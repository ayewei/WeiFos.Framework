using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.GaoDeng.APIModule
{

    /// <summary>
    /// 开具蓝票接口业务请求参数实体
    /// @date 2019-04-12
    /// </summary>
    [Serializable]
    public class APIInvoice
    {

        /// <summary>
        ///  string (15-20) 是 
        ///  销方纳税识别号	91469027MA5RH09M0R
        /// </summary>
        public string taxpayer_num { get; set; }

        /// <summary>
        ///  string (1-100) 否 
        ///  销方地址：广东省深圳市南山区大冲商务中心
        ///  销方纳税识别号	91469027MA5RH09M0R
        /// </summary>
        public string seller_address { get; set; }

        /// <summary>
        ///  string (7-20) 否 
        ///  销方电话	18285162583
        /// </summary>
        public string seller_tel { get; set; }

        /// <summary>
        ///  string (8-70) 否 
        ///  销方开户行 中国工商银行深圳中心支行
        /// </summary>
        public string seller_bank_name { get; set; }

        /// <summary>
        ///  string (8-30) 否 
        ///  销方银行账户	6212812402000333990
        /// </summary>
        public string seller_bank_account { get; set; }

        /// <summary>
        ///  int (1) 是 
        ///  购方类型，1:个人/事业单位；2:企业	 
        /// </summary>
        public int buyer_title_type { get; set; }

        /// <summary>
        /// string (15-50)	是 订单号 
        /// 订单号 gaodeng_1234567890	 
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// string (15-20)	是 订单号 
        /// 购方纳税识别号 110102053635536	 
        /// </summary>
        public string buyer_taxcode { get; set; }

        /// <summary>
        /// string (4-100)	是 订单号 
        /// 购方企业名称  海南高灯科技有限公司 
        /// </summary>
        public string buyer_title { get; set; }

        /// <summary>
        /// string (8-70)	否 订单号 
        /// 购方开户行，精确到支行，须与购方银行账号同时填入，不可只填其一 中国工商银行深圳中心区支行
        /// </summary>
        public string buyer_bank_name { get; set; }

        /// <summary>
        /// string (8-30)	否 订单号 
        /// 购方银行账号 621281564203214
        /// </summary>
        public string buyer_bank_account { get; set; }

        /// <summary>
        /// string (7-20) 否  
        /// 购方电话 29884888
        /// </summary>
        public string buyer_phone { get; set; }

        /// <summary>
        /// string(1-50) 否  
        /// 收票人名称(客户的名字)    张三
        /// </summary>
        public string taker_name { get; set; }

        /// <summary>
        /// string(11) 否  
        /// 收票人手机号(客户的手机) 18285162584
        /// </summary>
        public string taker_tel { get; set; }

        /// <summary>
        /// string(4-50) 否  
        /// 购买者邮箱(客户信息) akun@163.com
        /// </summary>
        public string buyer_email { get; set; }

        /// <summary>
        /// string(1-100) 否  
        /// 购买方地址   广东省深圳市南山区
        /// </summary>
        public string buyer_address { get; set; }

        /// <summary>
        /// string(1-130) 否  
        /// 发票备注  差额征税
        /// </summary>
        public string extra { get; set; }

        /// <summary>
        /// string(5-500) 否  
        /// http://商户通知地址/test/post
        /// </summary>
        public string callback_url { get; set; }

        /// <summary>
        /// string(1-8) 否  
        /// 收银员姓名，如果不填写，则默认根据开票方企业信息填入票面信息
        /// </summary>
        public string cashier { get; set; }

        /// <summary>
        /// string(1-8) 否  
        /// 复核员姓名，如果不填写，则默认根据开票方企业信息填入票面信息
        /// </summary>
        public string checker { get; set; }

        /// <summary>
        /// 开票员姓名，如果不填写，则默认根据开票方企业信息填入票面信息
        /// </summary>
        public string invoicer { get; set; }

        /// <summary>
        /// 行业分类
        /// 1通信、2餐饮、3交通、4支付平台、5票务/旅游、0其他
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 预留字段
        /// </summary>
        public string etr_data { get; set; }

        /// <summary>
        /// 发票种类编码
        /// 004:增值税专用发票，007:增值税普通发票，026：增值税电子发票，
        /// 025：增值税卷式发票, 032:区块链发票 默认为026
        /// </summary>
        public string invoice_type_code { get; set; }
         
    }


    /// <summary>
    /// 开票详情
    /// </summary>
    public class APIGoodsInfo
    {

        /// <summary>
        ///  string (1-90) 是 
        ///  商品名	农夫山泉
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///  string (2-19) 是 
        ///  税目编码：1020202000000000000 
        /// </summary>
        public string tax_code { get; set; }

        /// <summary>
        ///  string (1-20) 否 
        ///  税目类别	18285162583
        /// </summary>
        public string tax_type { get; set; }

        /// <summary>
        ///  string (1-40) 否 
        ///  商品规格 华为P10
        /// </summary>
        public string models { get; set; }

        /// <summary>
        ///  string (1-22) 否 
        ///  计量单位 个
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// string(10,2)是 
        /// 不含税商品金额,1000.50，不含税金额 = 不含税单价 * 数量， 结果保留两位小数
        /// </summary>
        public string total_price { get; set; }

        /// <summary>
        /// string (16,8) 是
        /// 商品数量（精确到8位）  	 
        /// </summary>
        public string total { get; set; }

        /// <summary>
        /// string (16,8)否
        /// 日常售卖的商品或服务一般是含税价格，对应不含税单价为 含税单价/(1 + 税率)。 
        /// 假如商品价格200元，税率 10%，则不含税价格为： 200/(1+10%) ≈ 181.82， 如果数量为1，保留两位小数即可，否则保留6位	 
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// string (10,2) 是
        /// 税率，范围0-1 0.17
        /// </summary>
        public string tax_rate { get; set; }

        /// <summary>
        /// string (10,2) 是
        /// 税额（精确到2位
        /// 1.70，税额=不含税金额*税率， 结果保留两位小数
        /// </summary>
        public string tax_amount { get; set; }

        /// <summary>
        /// string (10,2) 否
        /// 总的折扣金额；金额必须是负数；-10.0
        /// </summary>
        public string discount { get; set; }

        /// <summary>
        /// string (1) 否
        /// 零税率标识，默认为空,空：非零税率， 0：出口零税，1：免税，2：不征税，3普通零税率
        /// </summary>
        public string zero_tax_flag { get; set; }

        /// <summary>
        /// string (1) 否
        /// 优惠政策标志，空：不使用，1:使用
        /// </summary>
        public string preferential_policy_flag { get; set; }

        /// <summary>
        /// string (200) 否
        /// 增值税特殊管理，preferential_policy_flag 优惠政策标识位 1 时必填
        /// </summary>
        public string vat_special_management { get; set; }

        /// <summary>
        /// string (0-200) 否
        /// 预留字段，在详情中查看时，需要将数据json_decode之后，才能恢复到原来的数据格式,slkdchks
        /// </summary>
        public string etr_data { get; set; }

    }
}
