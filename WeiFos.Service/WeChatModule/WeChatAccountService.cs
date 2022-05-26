using System;
using WeiFos.Core;
using WeiFos.Entity.Common;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.WeChatModule.EntModule;
using WeiFos.Entity.WeChatModule.WXOpen;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 微信公众号 Service
    /// @author yewei 
    /// @date 2015-06-15
    /// </summary>
    public class WeChatAccountService : BaseService<WeChatAccount>
    {

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public string GetWeChatToken()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.ExecuteScalar("select token from tb_wx_account where id != 0").ToString();
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public WeChatAccount Get()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatAccount>("where id != @0", 0);
            }
        }



        /// <summary>
        /// 保存商户号（包含微信公众号支付配置）
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="account"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, WeChatAccount account, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    WeChatAccount wx_account = s.Get<WeChatAccount>("where id != @0", 0);
                    if (wx_account != null)
                    {
                        account.id = wx_account.id;
                        account.updated_user_id = user_id;
                        account.updated_date = DateTime.Now;
                        s.Update<WeChatAccount>(account);
                    }
                    else
                    {
                        int rand;
                        char code;
                        string randomcode = String.Empty;

                        //生成一定长度的ToKen
                        System.Random random = new Random();
                        for (int i = 0; i < 8; i++)
                        {
                            rand = random.Next();
                            if (rand % 3 == 0)
                            {
                                code = (char)('A' + (char)(rand % 26));
                            }
                            else
                            {
                                code = (char)('0' + (char)(rand % 10));
                            }
                            randomcode += code.ToString();
                        }

                        //设置账号ToKen
                        account.token = randomcode;

                        account.created_user_id = user_id;
                        account.created_date = DateTime.Now;
                        s.Insert<WeChatAccount>(account);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg) && imgmsg.IndexOf("#") != -1)
                    {
                        //图片名称
                        string filename = imgmsg.Split('#')[0];
                        //图片类型
                        string biztype = imgmsg.Split('#')[1];
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_type = @1  ", biztype, wx_account.id);
                        Img img = s.Get<Img>("where file_name = @0 and biz_type = @1 ", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = wx_account.id;
                            s.Update<Img>(img);
                        }
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 保存微信开发平台配置
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="open_setting"></param> 
        /// <returns></returns>
        public StateCode Save(long user_id, WXOpenSetting open_setting)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    WXOpenSetting entity = s.Get<WXOpenSetting>("where id > 0");
                    if (entity == null) s.Insert(open_setting);
                    else
                    {
                        open_setting.id = entity.id;
                        s.Update(open_setting);
                    }
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 保存企业号
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="account_ent"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, WeChatAccountEnt account_ent)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //企业号信息
                    WeChatAccountEnt wx_account_ent = s.Get<WeChatAccountEnt>("where id > 0");
                    if (wx_account_ent == null)
                    {
                        account_ent.created_user_id = user_id;
                        account_ent.created_date = DateTime.Now;
                        s.Insert(account_ent);
                    }
                    else
                    {
                        account_ent.id = wx_account_ent.id;
                        s.Update(account_ent);
                    }

                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 保存APP商户号
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, WeChatMerchant entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    WeChatMerchant mh = s.Get<WeChatMerchant>("where id > 0");
                    if (mh == null)
                    {
                        s.Insert<WeChatMerchant>(entity);
                    }
                    else
                    {
                        entity.id = mh.id;
                        s.Update<WeChatMerchant>(entity);
                    }

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 保存微信账号
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="account"></param>
        /// <param name="account_ent"></param>
        /// <param name="merchant"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, WeChatAccount account, WeChatAccountEnt account_ent, WeChatMerchant merchant, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    WeChatAccount wx_account = s.Get<WeChatAccount>("where id != @0", 0);
                    if (wx_account != null)
                    {
                        account.id = wx_account.id;
                        account.updated_user_id = user_id;
                        account.updated_date = DateTime.Now;
                        s.Update<WeChatAccount>(account);
                    }
                    else
                    {
                        int rand;
                        char code;
                        string randomcode = String.Empty;

                        //生成一定长度的ToKen
                        System.Random random = new Random();
                        for (int i = 0; i < 8; i++)
                        {
                            rand = random.Next();
                            if (rand % 3 == 0)
                            {
                                code = (char)('A' + (char)(rand % 26));
                            }
                            else
                            {
                                code = (char)('0' + (char)(rand % 10));
                            }
                            randomcode += code.ToString();
                        }

                        //设置账号ToKen
                        account.token = randomcode;

                        account.created_user_id = user_id;
                        account.created_date = DateTime.Now;
                        s.Insert<WeChatAccount>(account);
                    }

                    //企业号信息
                    WeChatAccountEnt wx_account_ent = s.Get<WeChatAccountEnt>("where id > 0");
                    if (wx_account_ent == null)
                    {
                        account_ent.created_user_id = user_id;
                        account_ent.created_date = DateTime.Now;
                        s.Insert(account_ent);
                    }
                    else
                    {
                        account_ent.id = wx_account_ent.id;
                        s.Update(account_ent);
                    }

                    WeChatMerchant mh = s.Get<WeChatMerchant>("where id > 0");
                    if (mh == null)
                    {
                        s.Insert<WeChatMerchant>(merchant);
                    }
                    else
                    {
                        merchant.id = mh.id;
                        s.Update<WeChatMerchant>(merchant);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg) && imgmsg.IndexOf("#") != -1)
                    {
                        //图片名称
                        string filename = imgmsg.Split('#')[0];
                        //图片类型
                        string biztype = imgmsg.Split('#')[1];
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_type = @1  ", biztype, wx_account.id);
                        Img img = s.Get<Img>("where file_name = @0 and biz_type = @1 ", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = wx_account.id;
                            s.Update<Img>(img);
                        }
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 公众号原始ID是否存在
        /// </summary>
        /// <param name="account_original_id"></param>
        /// <returns></returns>
        public int ExistOriginalId(string account_original_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<WeChatAccount>("where account_original_id = @0", account_original_id);
            }
        }

    }
}
