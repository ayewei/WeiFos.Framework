using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.OrgEntity
{
    /// <summary>
    /// 微信 部门集合
    /// 通过接口获取用户数据临时对象
    /// @author yewei 
    /// @date 2014-12-27
    /// </summary>
    [Serializable]
    public class WeChatEmployee : WXCodeError
    {

        /// <summary>
        /// 成员UserID。对应管理端的帐号，企业内必须唯一。
        /// 不区分大小写，长度为1~64个字节
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 成员所属部门id列表，仅返回该应用有查看权限的部门id
        /// </summary>
        public int[] department { get; set; }

        /// <summary>
        /// 职务信息；第三方仅通讯录应用可获取
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// 性别。0表示未定义，1表示男性，2表示女性
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 部门内的排序值，默认为0。数量必须和department一致，数值越大排序越前面。值范围是[0, 2^32)
        /// </summary>
        public int[] order { get; set; }

        /// <summary>
        /// 表示在所在的部门内是否为上级。
        /// 第三方仅通讯录应用可获取
        /// </summary>
        public int[] is_leader_in_dept { get; set; }

        /// <summary>
        /// 头像url。注：如果要获取小图将url最后的”/0”改成”/100”即可。第三方仅通讯录应用可获取
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 座机。第三方仅通讯录应用可获取
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 成员启用状态。1表示启用的成员，0表示被禁用。注意，服务商调用接口不会返回此字段
        /// </summary>
        public int enable { get; set; }

        /// <summary>
        /// 别名；第三方仅通讯录应用可获取
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// 扩展属性，第三方仅通讯录应用可获取
        /// Json格式对象
        /// </summary>
        //public string extattr { get; set; }

        /// <summary>
        ///  激活状态: 1=已激活，2=已禁用，4=未激活。
        ///  已激活代表已激活企业微信或已关注微工作台（原企业号）。
        ///  未激活代表既未激活企业微信又未关注微工作台（原企业号）
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///  员工个人二维码，扫描可添加为外部联系人；第三方仅通讯录应用可获取
        /// </summary>
        public string qr_code { get; set; }

        /// <summary>
        /// 成员对外属性，字段详情见对外属性；第三方仅通讯录应用可获取
        /// Json格式对象
        /// </summary>
        //public string external_profile { get; set; }

        /// <summary>
        /// external_position
        /// </summary>
        public string external_position { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

    }
}
