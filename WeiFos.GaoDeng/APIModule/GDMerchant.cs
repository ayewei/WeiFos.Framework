using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.GaoDeng.APIModule
{

    /// <summary>
    /// 高灯区块链电子发票 实体对象
    /// @date 2019-04-08
    /// </summary>
    public class GDMerchant
    {

        /// <summary>
        /// 企业名称
        /// </summary>
        public string taxpayer_name { get; set; }

        /// <summary>
        /// 纳税人识别号(税号)
        /// </summary>
        public string taxpayer_num { get; set; }

        /// <summary>
        /// 注册企业法人代表名称
        /// </summary>
        public string legal_person_name { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string bank_name { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string bank_account { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 开票人
        /// </summary>
        public string drawer { get; set; }

        /// <summary>
        /// 复核人
        /// </summary>
        public string reviewer { get; set; }

        /// <summary>
        /// 收款人
        /// </summary>
        public string payee { get; set; }
         
    }
}