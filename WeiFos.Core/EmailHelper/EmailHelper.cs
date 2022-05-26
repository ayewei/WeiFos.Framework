using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace WeiFos.Core.EmailHelper
{

    /// <summary>
    /// 发送邮件类
    /// @author yewei 
    /// @date 2013-07-04
    /// </summary>
    public class EmailHelper
    {




        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="send"></param>
        /// <param name="psw"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="smtp_host"></param>
        /// <param name="smtp_host_port"></param>
        /// <returns></returns>
        public static bool SendEmail(string send, string psw, string to, string subject, string body, string smtp_host, int smtp_host_port = 25)
        {
            bool send_success = false;
            try
            {
                //简单邮件传输协议类
                SmtpClient client = new SmtpClient();
                //邮件服务器地址
                client.Host = smtp_host;
                //邮件服务器端口号
                client.Port = smtp_host_port;
                //邮件发送方式:通过网络发送到SMTP服务器
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //凭证,发件人登录邮箱的用户名和密码
                client.Credentials = new System.Net.NetworkCredential(send, psw);

                //发件人Email,在邮箱是这样显示的,[发件人:小明<panthervic@163.com>;]
                MailAddress fromAddress = new MailAddress(send, "");
                //收件人Email,在邮箱是这样显示的, [收件人:小红<43327681@163.com>;]
                MailAddress toAddress = new MailAddress(to, "");
                //创建一个电子邮件类
                MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
                //邮件主题
                mailMessage.Subject = subject;
                //邮件内容
                mailMessage.Body = body;
                //邮件主题编码
                mailMessage.SubjectEncoding = Encoding.UTF8;
                //邮件正文编码System.Text.Encoding.GetEncoding("GB2312");
                mailMessage.BodyEncoding = Encoding.UTF8;
                //邮件内容是否为html格式
                mailMessage.IsBodyHtml = true;
                //邮件的优先级,有三个值:高(在邮件主题前有一个红色感叹号,表示紧急),低(在邮件主题前有一个蓝色向下箭头,表示缓慢),正常(无显示).
                mailMessage.Priority = MailPriority.High;

                //发送邮件
                client.Send(mailMessage);
                //client.SendAsync(mailMessage, "ojb");异步方法发送邮件,不会阻塞线程.

                send_success = true;
            }
            catch (Exception ex)
            {
                throw new Exception("邮件发送失败:原因" + ex.ToString());
            }

            return send_success;
        }





    }
}
