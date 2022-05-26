using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;



namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 粉丝标签中间表service
    /// @author yewei 
    /// @date 2015-01-05
    /// </summary>
    public class UserTagUniteService : BaseService<UserTagUnite>
    {

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="open_ids"></param>
        /// <param name="tag_ids"></param>
        /// <returns></returns>
        public StateCode AddUserTagUnites(string[] open_ids, int[] tag_ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    foreach (string uid in open_ids)
                    {
                        foreach (int tid in tag_ids)
                        {
                            //是否存在标签
                            int t_exist = s.Exist<WeChatUserTag>("where id = @0", tid);

                            //中间表重复数据过滤
                            int exist = s.Exist<UserTagUnite>("where user_id = @0 and user_tag_id = @1", uid, tid);

                            if (t_exist > 0 && exist == 0)
                            {
                                UserTagUnite tag_u = new UserTagUnite();
                                tag_u.open_id = uid;
                                tag_u.user_tag_id = tid;
                                s.Insert<UserTagUnite>(tag_u);
                            }
                        }
                    }

                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
            return StateCode.State_200;
        }



        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="tag_name"></param>
        /// <returns></returns>
        public void DeleteUserTagUnite(string open_id, string tag_name)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                WeChatUserTag userTag = s.Get<WeChatUserTag>(" where tag_name = @0 ", tag_name);
                if (userTag != null)
                {
                    s.ExecuteScalar("delete tb_pub_usertag where user_tag_id = @0 and open_id = @1", userTag.id, open_id);
                }
            }
        }


    }
}
