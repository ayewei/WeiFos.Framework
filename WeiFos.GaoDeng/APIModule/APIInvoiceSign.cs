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
    public class APIInvoiceSign : APIInvoice
    {
       
        /// <summary>
        /// 详细字段内容见下表格
        /// </summary>
        public string goods_info { get; set; }

    }
}
