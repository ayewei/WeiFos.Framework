using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 粉丝分组 类型
    /// @author yewei 
    /// @date 2015-01-04
    /// </summary>
    public class FansGroupType
    {
        /// <summary>
        /// 全部客户
        /// </summary>
        public const int all = 10;

        /// <summary>
        /// 今天新增客户
        /// </summary>
        public const int add_today = 15;

        /// <summary>
        /// 今天联系客户
        /// </summary>
        public const int contact_today = 20;

        /// <summary>
        /// 近7天联系客户
        /// </summary>
        public const int contact_seven = 25;

        /// <summary>
        /// 近30天联系客户
        /// </summary>
        public const int contact_thirty = 30;

        /// <summary>
        /// 近30天未联系客户
        /// </summary>
        public const int discontact_thirty = 35;


        /// <summary>
        /// 今天关注客户
        /// </summary>
        public const int attention_today = -10;

        /// <summary>
        /// 近7天关注客户
        /// </summary>
        public const int attention_seven = -15;

        /// <summary>
        /// 近30天关注客户
        /// </summary>
        public const int attention_thirty = -20;

        /// <summary>
        /// 30天前关注客户
        /// </summary>
        public const int attention_thirty_ago = -25;



        public static Dictionary<int, string> fansGroupTypeList = new Dictionary<int, string>()
        {
            {FansGroupType.all,"全部客户"},
            {FansGroupType.add_today,"今天新增客户"},
            {FansGroupType.contact_today,"今天联系客户"},
            {FansGroupType.contact_seven,"近7天联系客户"},
            {FansGroupType.contact_thirty,"近30天联系客户"},
            {FansGroupType.discontact_thirty,"近30天未联系客户"},

            {FansGroupType.attention_today,"今天关注客户"},
            {FansGroupType.attention_seven,"近7天关注客户"},
            {FansGroupType.attention_thirty,"近30天关注客户"},
            {FansGroupType.attention_thirty_ago,"30天前关注客户"}
        };

        /// <summary>
        /// 根据key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(int key)
        {
            foreach (var item in fansGroupTypeList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "";
        }

    }

}