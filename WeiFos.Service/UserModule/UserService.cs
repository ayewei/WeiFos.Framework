using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.SystemModule;
using WeiFos.Entity.UserModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using WeiFos.SDK.Model;

namespace WeiFos.Service.UserModule
{


    /// <summary>
    /// 用户 Service
    /// @author yewei
    /// @date 2016-03-20
    /// </summary>
    public class UserService : BaseService<User>
    {


        #region 登录、注册



        /// <summary>
        /// APP用户登录
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="pass_word"></param>
        /// <param name="ip"></param>
        /// <param name="signPackage"></param>
        /// <returns></returns>
        public User Login(string LoginName, string pass_word, string ip, SignPackage signPackage)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前用户是否存在
                User user = s.Get<User>("where login_name = @0", LoginName);
                if (user == null) user = s.Get<User>("where mobile = @0", LoginName);

                //如果用户为空
                if (user == null)
                {
                    user = new User();
                    user.login_code = StateCode.State_4;
                    return user;
                }

                //用户已被冻结
                if (user.state == 0)
                {
                    user = new User();
                    user.login_code = StateCode.State_209;
                    return user;
                }

                //如果用户用户密码都正确
                if (user != null && !user.psw.ToUpper().Equals(StringHelper.ConvertTo32BitSHA1(pass_word).ToUpper()))
                {
                    user.login_code = StateCode.State_4;
                    return user;
                }

                //登录成功
                user.login_code = StateCode.State_200;

                //获取用户图像
                Img img = s.Get<Img>("where biz_type = @0 and biz_id = @1", ImgType.User, user.id);
                if (img != null) user.head_img = img.getImgUrl();

                //登录是否成功
                s.ExcuteUpdate("update tb_user set last_ip = login_ip,login_ip = @0,last_time = login_time,login_time = @1,login_count = login_count + 1 where id = @2", ip, DateTime.Now, user.id);

                //重置当前用户所有令牌状态
                //s.ExcuteUpdate("update tb_user_token set is_enable = @0 where user_id = @1", false, user.id);

                signPackage.Token = Guid.NewGuid().ToString("N");
                UserToken token = new UserToken();
                token.last_time = DateTime.Now;
                token.created_date = DateTime.Now;
                token.os = signPackage.OS;
                token.user_id = user.id;
                token.imei = signPackage.IMEI;
                token.imsi = signPackage.IMSI;
                token.token = signPackage.Token;
                token.is_enable = true;
                //s.Insert(token);
                return user;
            }
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rpsw"></param>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public StateCode Register(User user, string rpsw, string vcode)
        {
            return Register(user, rpsw, vcode, null);
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rpsw"></param>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public StateCode Register(User user, string rpsw, string vcode, SignPackage signPackage)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //短信验证码不存在
                    SmsMessage sms = s.Get<SmsMessage>("where mobile = @0  and type = @1", user.mobile, 1);
                    if (sms == null) return StateCode.State_54;

                    //验证码错误
                    if (!vcode.Equals(sms.content)) return StateCode.State_53;

                    //计算时间间隔
                    TimeSpan t = DateTime.Now - sms.created_time;
                    if (t.TotalSeconds > 300) return StateCode.State_52;

                    //是否已经被注册 
                    int exist_m = s.Exist<User>("where mobile = @0", user.mobile);
                    if (exist_m > 0) return StateCode.State_202;

                    //两次密码不一致
                    if (!rpsw.Equals(user.psw)) return StateCode.State_203;

                    user.state = 1;
                    user.login_name = user.mobile;
                    user.created_date = DateTime.Now;
                    user.psw = StringHelper.ConvertTo32BitSHA1(user.psw);
                    s.Insert(user);

                    //用户详情
                    UserDetail detail = new UserDetail();
                    detail.user_id = user.id;
                    s.Insert(detail);

                    if (signPackage != null)
                    {
                        //重置当前用户所有令牌状态
                        s.ExcuteUpdate("update tb_user_token set is_enable = @0 where user_id = @1", false, user.id);
                        signPackage.Token = Guid.NewGuid().ToString("N");
                        UserToken token = new UserToken();
                        token.created_date = DateTime.Now;
                        token.os = signPackage.OS;
                        token.last_time = DateTime.Now;
                        token.user_id = user.id;
                        token.imei = signPackage.IMEI;
                        token.imsi = signPackage.IMSI;
                        token.token = signPackage.Token;
                        token.is_enable = true;
                        s.Insert(token);
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch(Exception ex)
                {
                    s.RollBack();

                    APILogs log = new APILogs();
                    log.content = "[Register]注册异常==》" + ex.ToString();
                    log.created_date = DateTime.Now;
                    s.Insert(log);

                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 微信端注册
        /// </summary>
        /// <param name="weChatUser"></param>
        /// <param name="mobile"></param>
        /// <param name="psw"></param>
        /// <param name="rpsw"></param>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public StateCode WeChatRegister(WeChatUser weChatUser, string mobile, string psw, string rpsw, string vcode)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //短信验证码不存在
                    SmsMessage sms = s.Get<SmsMessage>("where mobile = @0  and type = @1", mobile, 1);
                    if (sms == null) return StateCode.State_54;

                    //验证码错误
                    if (!vcode.Equals(sms.content)) return StateCode.State_53;

                    //计算时间间隔
                    TimeSpan t = DateTime.Now - sms.created_time;
                    if (t.TotalSeconds > 300) return StateCode.State_52;

                    //是否已经被注册 
                    int exist_l = s.Exist<User>("where login_name = @0", mobile);
                    if (exist_l > 0) return StateCode.State_202;

                    //是否已经被注册 
                    int exist_m = s.Exist<User>("where mobile = @0", mobile);
                    if (exist_m > 0) return StateCode.State_202;

                    //两次密码不一致
                    if (!rpsw.Equals(psw)) return StateCode.State_203;

                    User user = new User();
                    user.mobile = mobile;
                    user.psw = StringHelper.ConvertTo32BitSHA1(psw);
                    user.created_date = DateTime.Now;
                    s.Insert(user);

                    //设置用户ID
                    weChatUser.user_id = user.id;

                    //用户详情
                    UserDetail detail = new UserDetail();
                    detail.user_id = user.id;
                    s.Insert(detail);

                   

                    //设置密码
                    s.ExcuteUpdate("update tb_user set login_name = @0,mobile = @0,psw = @1 where id = @2  ", user.mobile, user.psw, user.id);

                    //如果在微信端打开，则绑定微信
                    if (weChatUser != null)
                    {
                        //修改用户状态
                        s.ExcuteUpdate("update tb_user set is_bind_wechat = @0  where id = @1 ", true, user.id);
                        s.ExcuteUpdate("update tb_wx_user set user_id = @0 where openid = @1", user.id, weChatUser.openid);
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    APILogs log = new APILogs();
                    log.content = "[WeChatRegister]注册异常==》" + ex.ToString();
                    log.created_date = DateTime.Now;
                    s.Insert(log);
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 令牌注销,退出登录
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public StateCode Cancel(string login_name, string token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前用户是否存在
                User user = s.Get<User>("where login_name = @0", login_name);
                if (user != null)
                {
                    UserToken userToken = s.Get<UserToken>("where token = @0 and user_id = @1", token, user.id);

                    //不存在该令牌token
                    if (userToken == null) return StateCode.State_204;

                    //无效的令牌token
                    if (!userToken.is_enable) return StateCode.State_205;

                    //重置当前用户所有令牌状态
                    s.ExcuteUpdate("update tb_user_token set is_enable = @0 ,updated_time = @1 where user_id = @2 ", 0, DateTime.Now, user.id);
                }
                return StateCode.State_200;
            }
        }



        #endregion



        /// <summary>
        /// 后台实时查询
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public User RTGet(string k)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                User user = s.Get<User>("where login_name = @0", k);
                if (user == null)
                {
                    user = s.Get<User>("where email = @0", k);
                }
                return user;
            }
        }



        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userDetail"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode UedateUser(UserDetail userDetail, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //获取用户详情
                    UserDetail detail = s.Get<UserDetail>("where user_id = @0", userDetail.user_id);

                    //用户详情ID
                    userDetail.id = detail.id;

                    s.Update(userDetail);

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.User, userDetail.user_id);

                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", userDetail.user_id, ImgType.User, imgmsg);
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
        /// 根据登录用户名获取
        /// </summary>
        /// <param name="loginname"></param>
        /// <returns></returns>
        public User GetByLoginName(string loginname)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<User>("where login_name = @0", loginname);
            }
        }


        /// <summary>
        /// 根据token获取用户信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public User GetByToken(string t)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                UserToken token = s.Get<UserToken>("where token = @0", t);
                if (token != null && token.is_enable)
                {
                    return s.Get<User>("where id = @0", token.user_id);
                }
            }
            return null;
        }


        /// <summary>
        /// 登录名是否存在
        /// </summary>
        /// <param name="moblile"></param>
        /// <returns></returns>
        public int ExistLoginName(string loginname)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<User>("where login_name = @0", loginname);
            }
        }


        /// <summary>
        /// 手机号码是否存在
        /// </summary>
        /// <param name="moblile"></param>
        /// <returns></returns>
        public int ExistMoblile(string moblile)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<User>("where mobile = @0", moblile);
            }
        }


        /// <summary>
        /// 找回密码（重置密码）
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="psw"></param>
        /// <returns></returns>
        public StateCode ResetPwd(string mobile, string psw)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {

                try
                {
                    s.StartTransaction();
                    var user = s.Get<User>("where mobile = @0", mobile);
                    if (user == null) { s.RollBack(); return StateCode.State_207; }
                    if (user.state == 0) { s.RollBack(); return StateCode.State_209; }
                    s.ExecuteScalar("update tb_user set psw = @0 where mobile = @1", psw, mobile);

                    var log = new APILogs();
                    log.created_date = DateTime.Now;
                    log.content = string.Format("用户{0}重置密码成功！", mobile);
                    s.Insert<APILogs>(log);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }




        /// <summary>
        /// 找回密码（重置密码）
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vcode"></param>
        /// <param name="psw"></param>
        /// <returns></returns>
        public StateCode ResetPassword(string mobile, string vcode, string psw)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {

                //短信验证码不存在
                SmsMessage sms = s.Get<SmsMessage>("where mobile = @0  and type = @1", mobile, (int)SendSmsType.ForgetPsw);
                if (sms == null) return StateCode.State_54;

                //验证码错误
                if (!vcode.Equals(sms.content)) return StateCode.State_53;

                //计算时间间隔
                TimeSpan t = DateTime.Now - sms.created_time;
                if (t.TotalSeconds > 300) return StateCode.State_52;

                try
                {
                    s.ExecuteScalar("update tb_user set psw = @0 where mobile = @1", StringHelper.ConvertTo32BitSHA1(psw), mobile);
                }
                catch
                {
                }
                return StateCode.State_200;
            }
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldpsw"></param>
        /// <param name="psw"></param>
        /// <returns></returns>
        public StateCode UpdatePsw(long userID, string oldpsw, string psw)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                psw = StringHelper.ConvertTo32BitSHA1(psw);
                oldpsw = StringHelper.ConvertTo32BitSHA1(oldpsw);
                int exist = s.Exist<User>("where id = @0 and psw = @1", userID, oldpsw);
                if (exist == 0) return StateCode.State_103;

                s.ExecuteScalar("update tb_user set psw = @0 where id = @1", psw, userID);
                return StateCode.State_200;
            }
        }


        /// <summary>
        /// 绑定新手机号码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="moblie"></param>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public StateCode BindNewMobile(long userID, string moblie, string vcode)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    int exist = s.Exist<User>("where mobile = @0", moblie);
                    if (exist == 1) return StateCode.State_206;

                    User user = s.Get<User>(userID);
                    if (user != null)
                    {
                        //短信验证码不存在
                        SmsMessage sms = s.Get<SmsMessage>("where mobile = @0 and content = @1 and Type = @2", moblie, vcode, (int)SendSmsType.BindNewMobile);
                        if (sms == null) return StateCode.State_51;

                        //验证码错误
                        if (!vcode.Equals(sms.content)) return StateCode.State_53;

                        //计算时间间隔
                        TimeSpan t = DateTime.Now - sms.created_time;
                        if (t.TotalSeconds > 300) return StateCode.State_52;

                        s.ExecuteScalar("update tb_user set mobile = @0,login_name = @0 where id = @1", moblie, userID);
                    }
                }
                catch
                {
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }



        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="psw"></param>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public StateCode ForGetPassword(string LoginName, string psw, string vcode)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                int exist = s.Exist<User>("where login_name = @0", LoginName);
                if (exist == 0) return StateCode.State_207;

                //短信验证码不存在
                SmsMessage sms = s.Get<SmsMessage>("where mobile = @0 and content = @1 and type = @2", LoginName, vcode, 10);
                if (sms == null || !vcode.Equals(sms.content)) return StateCode.State_53;

                //计算时间间隔
                TimeSpan t = DateTime.Now - sms.created_time;
                if (t.TotalSeconds > 300) return StateCode.State_52;

                s.ExecuteScalar("update tb_user set psw = @0 where login_name = @1", StringHelper.ConvertTo32BitSHA1(psw), LoginName);
                return StateCode.State_200;
            }
        }



        /// <summary>
        /// 设置会员状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public StateCode SetEnable(long id, int state)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    int exist = s.Exist<User>("where id = @0", id);
                    if (exist > 0)
                    {
                        s.ExcuteUpdate("update tb_user set state = @0 where id = @1", state, id);
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 手机号码是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public int ExistMobile(string mobile, long bid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<User>("where mobile = @0 and id != @1 ", mobile, bid);
            }
        }



        /// <summary>
        /// 邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int ExistEmail(string email, long bid = 0)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<User>("where email = @0 and id != @1", email, bid);
            }
        }



        /// <summary>
        /// 邮箱是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public StateCode BindNewEmail(long id, string email, string vcode)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    int exist = s.Exist<User>("where id = @0", id);
                    if (exist > 0)
                    {
                        s.ExcuteUpdate("update tb_user set email = @0 where id = @1", email, id);
                    }

                    //短信验证码不存在
                    SmsMessage sms = s.Get<SmsMessage>("where email = @0 and content = @1 ", email, vcode);
                    if (sms == null) return StateCode.State_51;

                    //验证码错误
                    if (!vcode.Equals(sms.content)) return StateCode.State_53;

                    //计算时间间隔
                    TimeSpan t = DateTime.Now - sms.created_time;
                    if (t.TotalSeconds > 1800) return StateCode.State_52;

                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }






        #region app function


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public dynamic GetUserDetails(long user_id, string token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //获取用户信息
                    User user = s.Get<User>("where id = @0", user_id);

                    //获取用户详情
                    UserDetail detail = s.Get<UserDetail>("where user_id = @0", user.id);
                    if (detail == null)
                    {
                        //用户详情
                        detail = new UserDetail();
                        detail.user_id = user.id;
                        s.Insert(detail);
                    }

                    string imgurl = "";
                    Img img = s.Get<Img>("where biz_type = @0 and biz_id = @1 ", ImgType.User, user.id);
                    if (img != null)
                    {
                        imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? "" : img.getImgUrl();
                    }
                    else
                    {
                        object url = s.ExecuteScalar("select headimgurl from tb_wx_user where user_id = @0", user_id);
                        if (url != null) imgurl = url.ToString();
                    }

                    return new
                    {
                        Token = token,
                        user_id = user.id,
                        //图像地址
                        headimg = imgurl,
                        //昵称
                        nickname = detail.nickname ?? "",
                        //邮箱
                        email = user.email ?? "",
                        //手机号码
                        mobile = user.mobile ?? "",
                        //性别
                        sex = detail.sex, 
                        is_email_auth = string.IsNullOrEmpty(user.email) ? false : true,
                        province = detail.province ?? "",
                        city = detail.city ?? "",
                        area = detail.area ?? "", 
                        birth = detail.birth == null ? DateTime.Parse("1900-01-01") : detail.birth
                    };
                }
                catch 
                {
                    return StateCode.State_500;
                }
            }
        }





        /// <summary>
        /// 修改某个字段
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public StateCode UedateUserByKey(long user_id, string key, string val)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    switch (key)
                    {
                        case "imgmsg":
                            //去除重复图片
                            s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.User, user_id);
                            //去除重复图片
                            s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", user_id, ImgType.User, val);
                            break;
                        case "nickname":
                            s.ExcuteUpdate("update tb_user_details set name = @0 where user_id = @1 ", val, user_id);
                            break;
                        case "email":
                            s.ExcuteUpdate("update tb_user set email = @0 where id = @1 ", val, user_id);
                            break;
                        case "sex":
                            s.ExcuteUpdate("update tb_user_details set sex = @0 where user_id = @1 ", val, user_id);
                            break;
                        case "birth":
                            s.ExcuteUpdate("update tb_user_details set birth = @0 where user_id = @1 ", val, user_id);
                            break;
                        case "p_signature":
                            s.ExcuteUpdate("update tb_user_details set p_signature = @0 where user_id = @1 ", val, user_id);
                            break;
                        case "province#city#area":
                            string[] arr = val.Split('#');
                            string province = arr[0];
                            string city = arr[1];
                            string area = arr[2];

                            s.ExcuteUpdate("update tb_user_details set province = @0 where user_id = @1 ", province, user_id);
                            s.ExcuteUpdate("update tb_user_details set city = @0 where user_id = @1 ", city, user_id);
                            s.ExcuteUpdate("update tb_user_details set area = @0 where user_id = @1 ", area, user_id);
                            break;
                        case "city":
                            s.ExcuteUpdate("update tb_user_details set city = @0 where user_id = @1 ", val, user_id);
                            break;
                        case "area":
                            s.ExcuteUpdate("update tb_user_details set area = @0 where user_id = @1 ", val, user_id);
                            break;
                    }

                    return StateCode.State_200;
                }
                catch 
                {
                    return StateCode.State_500;
                }
            }
        }




        #endregion






    }
}