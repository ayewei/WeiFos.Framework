using WeiFos.ORM.Data;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.ShopSettingModule;
 

namespace WeiFos.Service.ShopSettingModule
{
    /// <summary>
    /// 商城信息 Service
    /// @author yewei 
    /// @date 2015-03-19
    /// </summary>
    public class MallMsgService : BaseService<MallMsg>
    {

        /// <summary>
        /// 获取商城信息
        /// </summary>
        /// <returns></returns>
        public MallMsg Get()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<MallMsg>("", "");
            }
        }

         

    }
}
