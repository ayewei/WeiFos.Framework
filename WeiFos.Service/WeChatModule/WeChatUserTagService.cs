using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using System;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.WeChatModule
{

    /// <summary>
    /// 默认回复设置Service
    /// @author yewei 
    /// @date 2015-01-04
    /// </summary>
    public class WeChatUserTagService : BaseService<WeChatUserTag>
    {

        /// <summary>
        /// 删除用户标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteTag(int id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //删除用户标签 
                s.ExecuteScalar("delete tb_wx_usertag where id = @0", id);
                //删除中间表数据
                s.ExecuteScalar("delete tb_wx_usertag_u where user_tag_id = @0", id);
            }
        }


        /// <summary>
        /// 粉丝标签
        /// </summary>
        /// <param name="user_tag"></param>
        public StateCode Save(WeChatUserTag user_tag)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (user_tag.id == 0)
                    {
                        user_tag.created_date = DateTime.Now;
                        s.Insert<WeChatUserTag>(user_tag);
                    }
                    else
                    {
                        s.Update<WeChatUserTag>(user_tag);
                    }
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }




        
    }
}
