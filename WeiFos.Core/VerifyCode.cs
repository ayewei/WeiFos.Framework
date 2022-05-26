using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace WeiFos.Core
{
    /// <summary>
    /// 验证码处理
    /// @Author yewei
    /// </summary>
    public class VerifyCode
    {
        const string Chars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjklmnpqrstuvwxyz";

        //生成一定长度的验证码
        static Random random = new Random();

        ///  <summary>
        ///  生成随机码
        ///  </summary>
        ///  <param  name="length">随机码个数</param>
        ///  <returns></returns>
        public static string CreateRandomCode(int length)
        {
            int rand;
            char code;
            string randomcode = String.Empty;

            for (int i = 0; i < length; i++)
            {
                rand = random.Next();
                code = Chars[rand % Chars.Length];
                //if (rand % 3 == 0)
                //{
                //    code = (char)('A' + (char)(rand % 26));
                //}
                //else
                //{
                //    code = (char)('0' + (char)(rand % 10));
                //}

                randomcode += code.ToString();
            }
            return randomcode;
        }

        /// <summary>
        /// 生成纯数字 随机码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomNum(int length)
        {
            int rand;
            char code;
            string randomcode = "";
            string Nums = "0123456789";

            for (int i = 0; i < length; i++)
            {
                rand = random.Next();
                code = Nums[rand % Nums.Length];
                randomcode += code.ToString();
            }
            return randomcode;
        }

        ///  <summary>
        ///  创建随机码图片
        ///  </summary>
        ///  <param  name="randomcode">随机码</param>
        public static byte[] CreateImage(string randomcode)
        {
            //随机转动角度
            int randAngle = 45;

            int mapwidth = (int)(randomcode.Length * 23);

            //创建图片背景
            Bitmap map = new Bitmap(mapwidth, 28);
            Graphics graph = Graphics.FromImage(map);

            try
            {
                //清除画面，填充背景
                graph.Clear(Color.AliceBlue);

                //画一个边框
                //graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, map.Width - 1, map.Height - 1);

                //模式
                //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Random rand = new Random();

                //背景噪点生成
                Pen blackPen = new Pen(Color.LightGray, 0);
                for (int i = 0; i < 50; i++)
                {
                    int x = rand.Next(0, map.Width);
                    int y = rand.Next(0, map.Height);
                    graph.DrawRectangle(blackPen, x, y, 1, 1);
                }

                //验证码旋转，防止机器识别
                char[] chars = randomcode.ToCharArray();//拆散字符串成单字符数组

                //文字距中
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                //定义颜色
                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
                //定义字体
                string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };

                for (int i = 0; i < chars.Length; i++)
                {
                    int cindex = rand.Next(7);
                    int findex = rand.Next(5);

                    Font f = new System.Drawing.Font(font[findex], 13, System.Drawing.FontStyle.Bold);
                    Brush b = new System.Drawing.SolidBrush(c[cindex]);
                    Point dot = new Point(16, 16);

                    float angle = rand.Next(-randAngle, randAngle);//转动的度数

                    graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
                    graph.RotateTransform(angle);
                    graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                    graph.RotateTransform(-angle);//转回去
                    graph.TranslateTransform(2, -dot.Y);//移动光标到指定位置
                }

                //生成图片
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                //输出图片流
                return ms.ToArray();

            }
            finally
            {
                graph.Dispose();
                map.Dispose();
            }
        }


    }
}

