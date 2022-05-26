using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ReplyModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Restrictions;

namespace WeiFos.Service.ReplyModule
{
    /// <summary>
    /// 回复关键字Service
    /// @author yewei 
    /// @date 2013-04-27
    /// </summary>
    public class KeyWordService : BaseService<KeyWord>
    {

        /// <summary>
        /// 查询关键词组
        /// </summary>
        /// <param name="biz_id">业务ID</param>
        /// <param name="biz_type">业务类型</param>
        /// <returns>返回List类型</returns>
        public List<KeyWord> GetKeywordGroup(long biz_id, string biz_type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<KeyWord>("@tb_rpy_keywords where biz_id = @0 and biz_type = @1 ", biz_id, biz_type);
            }
        }


        /// <summary>
        /// 查询关键词组
        /// </summary>
        /// <param name="biz_id"></param>
        /// <param name="biz_type"></param>
        /// <returns></returns>
        public string GetKeywords(long biz_id, string biz_type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<KeyWord> keyWords = s.List<KeyWord>("where biz_id = @0 and biz_type = @1", biz_id, biz_type);
                if (keyWords != null && keyWords.Count > 0)
                {
                    return string.Join(" ", keyWords.Select(k => k.keyword).ToArray());
                }
                return "";
            }
        }


        /// <summary>
        /// 查询关键词是否存在
        /// </summary>
        /// <param name="keywords">关键词组</param>
        /// <param name="bid">排除业务ID</param>
        /// <returns></returns>
        public int CheckIsExsit(string keywords, long biz_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    string[] KeywordArray = keywords.Trim().Split(' ').Distinct().ToArray();
                    string filter = StringHelper.ArrayToString(KeywordArray, ",", true);

                    foreach (string k in KeywordArray)
                    {
                        if (biz_id != 0)
                        {
                            return s.Exist<KeyWord>("where keyword in (" + filter + ") and biz_id != " + biz_id);
                        }
                        else
                        {
                            return s.Exist<KeyWord>("where keyword in (" + filter + ")");
                        }
                    }
                }
                return 0;
            }
        }



        /// <summary>
        /// 保存关键字
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="matchtype"></param>
        /// <param name="biz_id"></param>
        /// <param name="biz_type"></param>
        public void Save(string keywords, int keywordtype, long biz_id, string biz_type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    s.ExcuteUpdate("delete tb_rpy_keywords where biz_id=@0 and biz_type=@1 ", biz_id, biz_type);

                    if (!string.IsNullOrEmpty(keywords))
                    {
                        string[] keywordArray = keywords.ToString().Trim().Split(' ');
                        foreach (string k in keywordArray)
                        {
                            if (k != string.Empty)
                            {
                                KeyWord entity = new KeyWord()
                                {
                                    keyword = k,
                                    biz_id = biz_id,
                                    biz_type = biz_type,

                                    updated_date = DateTime.Now,
                                    created_date = DateTime.Now
                                };
                                ServiceIoc.Get<KeyWordService>().Insert(entity);
                            }
                        }
                    }
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }

        /// <summary>
        /// 处理微信请求关键词
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public KeyWord GetKeyWordMsg(string keyword)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<KeyWord>("where keyword = @0 ", keyword);
            }
        }


    }
}
