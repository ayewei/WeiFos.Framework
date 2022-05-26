using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.Common;
using WeiFos.Entity.SystemModule;
using WeiFos.Entity.Enums;
using WeiFos.Core;
using WeiFos.Entity.UserModule;
using WeiFos.Service.LogsModule;
using WeiFos.Entity.SiteSettingModule;
using WeiFos.Core.EmailHelper;
using WeiFos.Entity.BizTypeModule;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Exceptions;
using Newtonsoft.Json;
using WeiFos.Core.SettingModule;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 发送短信 service
    /// @author yewei
    /// @date 2016-03-07
    /// </summary>
    public class SmsMessageService : BaseService<SmsMessage>
    {


        /// <summary>
        /// 保存短信
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public StateCode SaveSms(SmsMessage message)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (string.IsNullOrEmpty(message.mobile)) return StateCode.State_50;

                    s.ExcuteUpdate("delete tb_sms where mobile = @0 and Type = @1", message.mobile, message.type);
                    message.created_time = DateTime.Now;
                    s.Insert<SmsMessage>(message);

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
        /// 根据手机号码获取短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public SmsMessage Get(string mobile, int type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //状态
                return s.Get<SmsMessage>("where mobile = @0 and type = @1", mobile, type);
            }
        }



        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="type">验证类型</param>
        /// <returns></returns>
        public StateCode Validate(string mobile, string code, int type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (string.IsNullOrEmpty(mobile)) return StateCode.State_50;
                    if (string.IsNullOrEmpty(code)) return StateCode.State_55;

                    //短信验证码不存在
                    SmsMessage sms = s.Get<SmsMessage>("where mobile = @0  and type = @1", mobile, type);
                    if (sms == null) return StateCode.State_53;
                    //验证码已经被使用过了
                    if (sms.content.Contains("*")) return StateCode.State_56;
                    //验证码错误
                    if (!code.Equals(sms.content)) return StateCode.State_53;

                    //计算时间间隔
                    TimeSpan t = DateTime.Now - sms.created_time;
                    if (t.TotalSeconds > 300) return StateCode.State_52;

                    s.ExcuteUpdate("update tb_sms set content='*'+content where mobile = @0 and type = @1", mobile, type);

                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="domain_name">回调验证邮箱的域名</param>
        /// <param name="user_id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public StateCode SendEmail(string domain_name, long user_id, string email)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(email)) return StateCode.State_218;

                    //是否存在邮箱
                    int exist = s.Exist<User>("where email = @0", email);
                    if (exist > 0) return StateCode.State_215;

                    //生成验证码
                    string code = StringHelper.CreateRandomCode(5);

                    //发送邮件配置参数
                    ConfigParam smtpServer = s.Get<ConfigParam>("where config_key = @0 ", "EmailConfig");

                    //参数是否存在
                    if (smtpServer != null && !string.IsNullOrWhiteSpace(smtpServer.config_value))
                    {
                        //邮件配置模板
                        WebIntroduction webintr = s.Get<WebIntroduction>("where type = @0", WebIntroductionType.EmailTmp);
                        if (webintr == null) return StateCode.State_224;

                        string[] arr = smtpServer.config_value.Split('#');
                        string smtp = arr[0];
                        string send = arr[1];
                        string psw = arr[2];

                        string url = string.Format("{0}/passport/BindEmail?email={1}&code={2}&ukey={3}", domain_name, email, code, StringHelper.GetEncryption(user_id.ToString(), code));
                        string body = string.Format(webintr.context, email, url, code);

                        bool send_success = EmailHelper.SendEmail(send, psw, email, "微狐信息科技-邮件认证", body, smtp, 25);
                        if (!send_success)
                        {
                            return StateCode.State_225;
                        }
                        else
                        {
                            SmsMessage sms = new SmsMessage();
                            sms.email = email;
                            sms.created_time = DateTime.Now;
                            sms.content = code;

                            s.ExcuteUpdate("delete tb_sms where email = @0 ", email);
                            s.Insert(sms);
                        }
                    }

                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        #region 短信验证码以及通知       


        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="moblile"></param>
        /// <param name="type"></param>
        /// <param name="data">默认#拼接参数</param>
        /// <returns></returns>
        public StateCode GetSMSCode(string moblile, int type, string data = "")
        {
            string code = StringHelper.CreateRandomCode(4);
            bool is_test = ConfigManage.AppSettings<bool>("AppSettings:IsTestSendSms");
            if (is_test) code = "8888";

            StateCode state = SendSms(moblile, code, type);
            if (StateCode.State_200 == state && !is_test)
            {
                IClientProfile profile = DefaultProfile.GetProfile("default", ConfigManage.AppSettings<string>("AppSettings:SMSKey"), ConfigManage.AppSettings<string>("AppSettings:SMSKeySecret"));
                DefaultAcsClient client = new DefaultAcsClient(profile);
                CommonRequest request = new CommonRequest();
                request.Method = MethodType.POST;
                request.Domain = "dysmsapi.aliyuncs.com";
                request.Version = "2017-05-25";
                request.Action = "SendSms"; 
                request.AddQueryParameters("PhoneNumbers", moblile);
                request.AddQueryParameters("SignName", "微狐信息科技");
                // request.Protocol = ProtocolType.HTTP;

                if (type == (int)SendSmsType.Register)
                {
                    //注册
                    request.AddQueryParameters("TemplateCode", ConfigManage.AppSettings<string>("AppSettings:SMSTmpRegister"));
                    request.AddQueryParameters("TemplateParam", JsonConvert.SerializeObject(new { code, product = "微狐信息科技" }));
                }
                else if (type == (int)SendSmsType.ForgetPsw)
                {
                    //忘记密码
                    request.AddQueryParameters("TemplateCode", ConfigManage.AppSettings<string>("AppSettings:SMSTmpForgetPsw"));
                    request.AddQueryParameters("TemplateParam", JsonConvert.SerializeObject(new { code, product = "微狐信息科技" }));
                }
                else if (type == (int)SendSmsType.BindNewMobile)
                {
                    //绑定新手机号
                    request.AddQueryParameters("TemplateCode", ConfigManage.AppSettings<string>("AppSettings:SMSTmpBindNewMob"));
                    request.AddQueryParameters("TemplateParam", JsonConvert.SerializeObject(new { code, product = "微狐信息科技" }));
                }

                try
                {
                    CommonResponse response = client.GetCommonResponse(request);
                    Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                }
                catch (ServerException e)
                {
                    state = StateCode.State_500;
                    Console.WriteLine(e);
                }
                catch (ClientException e)
                {
                    state = StateCode.State_500;
                    Console.WriteLine(e);
                }
            }

            return state;
        }




        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="moblile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public StateCode SendSms(string moblile, string code, int type)
        {
            if (string.IsNullOrEmpty(moblile)) return StateCode.State_50;
            if (type == 0) throw new Exception("短信业务类型异常");

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //注册
                    if (type == (int)SendSmsType.Register)
                    {
                        int exist = s.Exist<User>("where mobile = @0 or login_name = @0", moblile);
                        if (exist == 1)
                        {
                            return StateCode.State_206;
                        }
                    }

                    //忘记密码
                    if (type == (int)SendSmsType.ForgetPsw)
                    {
                        int exist = s.Exist<User>("where mobile = @0", moblile);
                        if (exist == 0) return StateCode.State_207;
                    }

                    //绑定新手机
                    if (type == (int)SendSmsType.BindNewMobile)
                    {
                        int exist = s.Exist<User>("where mobile = @0 or login_name = @0", moblile);
                        if (exist == 1) return StateCode.State_206;
                    }


                    s.StartTransaction();

                    SmsMessage sms = new SmsMessage();
                    sms.type = type;
                    sms.mobile = moblile;
                    sms.created_time = DateTime.Now;
                    sms.content = code;

                    s.ExcuteUpdate("delete tb_sms where mobile = @0 and Type = @1", sms.mobile, sms.type);
                    s.Insert(sms);

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


        #endregion



    }
}
