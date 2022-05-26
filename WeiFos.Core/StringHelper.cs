using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WeiFos.Core.SettingModule;

namespace WeiFos.Core
{
    /// <summary>
    /// @Author yewei 
    /// 字符处理对象
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// UrlEncode转义后 特殊符号集合
        /// </summary>
        private static Dictionary<string, string> specialSymbols = new Dictionary<string, string>()
        {
            { "+","%20"}//空格转义后变为+，特殊处理为%20
        };

        public static bool IsMatch(this string s, string pattern)
        {
            return Regex.IsMatch(s, pattern);
        }

        static Dictionary<string, string> keywordDic = new Dictionary<string, string>()
        {
            {"'","&apos;" },
            { " "," "},/*解决全角空格导入后显示?字符的问题*/
        };

        /// <summary>
        /// 替换危险字符
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ReplaceDangerChar(this string src)
        {
            foreach (var item in keywordDic)
            {
                src = src.Replace(item.Key, item.Value);
            }
            return src;
        }

        public static string UrlEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }


        /// <summary>
        /// URL特效符号处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncodeSymbolsReplace(string str)
        {
            foreach(var a in specialSymbols)
            {
                str = Regex.Replace(str, @"\" + a.Key + "", "%20");
            }
            return str;
        }

        public static string UrlDecode(string str)
        {
            return System.Web.HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int length)
        {
            int rand;
            char code;
            string randomcode = String.Empty;

            //生成一定长度的验证码
            System.Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                rand = random.Next();
                code = (char)('0' + (char)(rand % 10));
                randomcode += code.ToString();
            }
            return randomcode;
        }

        /// <summary>
        /// 获取随机验证码
        /// 带字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomCode(int length)
        {
            int rand;
            char code;
            string randomcode = String.Empty;

            //生成一定长度的验证码
            System.Random random = new Random();
            for (int i = 0; i < length; i++)
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
            return randomcode;
        }

        /// <summary>
        /// 加密成３２位(MD5)
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>32位字符串</returns>
        public static string ConvertTo32BitMD5(string str)
        {
            string strMd5 = StringHelper.MD5(str);
            return strMd5;
        }

        /// <summary>
        /// 加密成３２位(SHA1)
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>位字符串</returns>
        public static string ConvertTo32BitSHA1(string str)
        {
            string strDecr = DecryptStr(str);
            string strSHA1 = SHA1(strDecr);
            return strSHA1;
        }

  
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="Text">要加密的字符串</param>
        /// <returns>密文</returns>
        public static string MD5(string str)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider check;
                check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] somme = check.ComputeHash(buffer);
                string ret = "";
                foreach (byte a in somme)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("X");//16进制
                    else
                        ret += a.ToString("X");
                }
                return ret.ToLower();
            }
            catch
            {
                throw;
            }
        }

 

        /// <summary>
        /// 字符串SHA-1加密 
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            try
            {
                System.Security.Cryptography.SHA1 check;
                check = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                byte[] somme = check.ComputeHash(buffer);
                string ret = "";
                foreach (byte a in somme)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("X");//16进制
                    else
                        ret += a.ToString("X");
                }
                return ret.ToLower();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 顺序减1解码 (偏移量)
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static string DecryptStr(string rs)
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] - 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }

        /// <summary>
        /// 倒序加1加密 （偏移量）
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static string EncryptStr(string rs)
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] + 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }

        /// <summary>
        /// Distinct返回关键词数组
        /// </summary>
        /// <param name="keywords">关键词组</param>
        /// <returns></returns>
        public static string[] GetDistinctStrToArray(string keywords)
        {
            if (!string.IsNullOrEmpty(keywords))
                return keywords.Trim().Split(' ').Distinct().ToArray();
            return null;
        }


        /// <summary>
        /// 去掉字符串中的数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNumber(string key)
        {
            return System.Text.RegularExpressions.Regex.Replace(key, @"\d", "");
        }


        /// <summary>
        /// 去掉字符串中的非数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNotNumber(string key)
        {
            return System.Text.RegularExpressions.Regex.Replace(key, @"[^\d]*", "");
        }

        /// <summary>
        /// 防SQL注入 通过返回true 未通过返回false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ValidateSqlInput(string str)
        {
            bool isNaN = true;
            if (string.IsNullOrEmpty(str)) //如果字符串为空，直接返回。
            {
                isNaN = true;
            }
            else
            {
                string SQL_injdata = "";
                SQL_injdata = ";,>,<,=,--,sp_,xp_,|,dir,cmd,^,+,$,',or,copy,format,and,exec,insert,select,delete,update,count,*,%,chr,mid,master,truncate,char,declare,cast,convert";// 把“:”排除,(,)
                string[] SQL_inj = SQL_injdata.Split(',');
                for (int i = 0; i < SQL_inj.Length; i++)
                {
                    if (Convert.ToBoolean(str.ToLowerInvariant().Contains(SQL_inj[i].ToString())))
                    {
                        isNaN = false;
                        break;
                    }
                }
            }
            return isNaN;
        }

        /// <summary>
        /// 处理含有英文逗号string字符串转化成Int数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] StringToIntArray(string str)
        {
            int[] Ids;
            int _id = 0;
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return new int[] { };
                }

                if (str.IndexOf(',') != -1)
                {
                    Ids = str.Split(',').Select(id => int.TryParse(id, out _id) ? _id : 0).ToArray();
                }
                else
                {
                    int.TryParse(str, out _id);
                    Ids = new int[] { _id };
                }
            }
            catch
            {
                Ids = new int[] { };
            }
            return Ids;
        }

        /// <summary>
        /// 处理含有英文逗号string字符串转化成string数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] StringToArray(string str)
        {
            string[] Ids;
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return new string[] { };
                }

                if (str.IndexOf(',') != -1)
                {
                    Ids = str.Split(',').ToArray();
                }
                else
                {
                    Ids = new string[] { str };
                }
            }
            catch
            {
                Ids = new string[] { };
            }
            return Ids;
        }

 

        /// <summary>
        /// 处理数组转成字符串格式
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="split">分隔符</param>
        /// <param name="quote">每个值加单引号</param>
        /// <returns></returns>
        public static string ArrayToString(Array array, string split, bool quote)
        {
            string str = string.Empty;
            try
            {
                foreach (var a in array)
                {
                    if (quote)
                        str += "'" + a + "'" + split;
                    else
                        str += a + split;
                }
                if (str.Length > 0 && !string.IsNullOrEmpty(split))
                    str = str.Substring(0, str.Length - 1);
                return str;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 处理含有英文逗号string
        /// 字符串转化成Long数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long[] StringToLongArray(string str)
        {
            long[] Ids;
            long _id = 0;
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return new long[] { };
                }

                if (str.IndexOf(',') != -1)
                {
                    Ids = str.Split(',').Select(id => long.TryParse(id, out _id) ? _id : 0).ToArray();
                }
                else
                {
                    long.TryParse(str, out _id);
                    Ids = new long[] { _id };
                }
            }
            catch
            {
                Ids = new long[] { };
            }
            return Ids;
        }



        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="slength">截取长度</param>
        /// <returns></returns>
        public static string GetSubString(string str, int slength)
        {
            string strlength = "";
            if (str != null)
            {
                if (str.Length > slength)
                {
                    strlength = str.Substring(0, slength) + "...";
                }
                else
                {
                    strlength = str;
                }
            }

            return strlength;
        }

        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string bSubstring(string s, int length)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(s);
            int n = 0;  //  表示当前的字节数
            int i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < length; i++)
            {

                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {

                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }

            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                    i = i - 1;
                //  该UCS2字符是字母或数字，则保留该字符
                else
                    i = i + 1;
            }

            if (bytes.Length > length)
            {
                return System.Text.Encoding.Unicode.GetString(bytes, 0, i).ToString() + "...";
            }
            return System.Text.Encoding.Unicode.GetString(bytes, 0, i).ToString();
        }


        //// <summary>
        /// 截取字符串，不限制字符串长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="len">每行的长度，多于这个长度自动换行</param>
        /// <returns></returns>
        public static string CutStr(string str, int len)
        {
            string s = "";
            for (int i = 0; i < str.Length; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len;
                if (i != 0 && i <= last)
                {
                    if (r == 0)
                    {
                        s += str.Substring(i - len, len) + "<br>";
                    }
                }
                else if (i > last)
                {
                    s += str.Substring(i - 1);
                    break;
                }
            }
            return s;
        }


        /// <summary>
        /// 截取字符串并限制字符串长度，多于给定的长度＋。。。
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="len">每行的长度，多于这个长度自动换行</param>
        /// <param name="max">输出字符串最大的长度</param>
        /// <returns></returns>
        public static string CutStr(string str, int len, int max)
        {
            string s = "";
            string sheng = "";
            if (str.Length > max)
            {
                str = str.Substring(0, max);
                sheng = "";
            }
            for (int i = 0; i < str.Length; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len;
                if (i != 0 && i <= last)
                {

                    if (r == 0)
                    {
                        s += str.Substring(i - len, len) + "<br>";
                    }
                }
                else if (i > last)
                {
                    s += str.Substring(i - 1);
                    break;
                }
            }
            return s + sheng;
        }


        /// <summary>
        /// 获取隐藏手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetHideMobile(string str)
        {
            return Regex.Replace(str, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
        }



        /// <summary>
        /// 创建0到9,a到z,A到Z随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateNoncestr(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }


        /// <summary>
        /// 对比sku组合字符串
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool CompSkuStr(string str1, string str2)
        {
            //打乱顺序对比
            if (str1.IndexOf(",") != -1 && str2.IndexOf(",") != -1)
            {
                string[] arr1 = str1.Split(',');
                string[] arr2 = str2.Split(',');

                int i = 0;
                foreach (string s1 in arr1)
                {
                    foreach (string s2 in arr2)
                    {
                        if (s1.Equals(s2))
                        {
                            i++;
                            break;
                        }
                    }
                }

                if (i == arr1.Count() && i == arr2.Count())
                {
                    return true;
                }
            }

            if (str1 != null && str1.Equals(str2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region 加密字符串
        public static string GetEncryption(string strValue, string key)
        {
            //加密标准算法的对象
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            //建立加密对象的密钥和偏移量
            provider.Key = Encoding.ASCII.GetBytes(ConvertTo32BitSHA1(key).Substring(0, 8));
            //原文使用Encoding.ASCII方法的GetBytes方法
            provider.IV = Encoding.ASCII.GetBytes(ConvertTo32BitSHA1(key).Substring(0, 8));
            //将要加密的字符放到byte数组中
            byte[] bytes = Encoding.UTF8.GetBytes(strValue);
            //输入的文本必须是英文文本
            MemoryStream stream = new MemoryStream();
            //定义将数据连接到加密转换的流
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);//将当前字节写入到流中
            stream2.FlushFinalBlock();//清除缓存区
            StringBuilder builder = new StringBuilder();
            //循环遍历每个字节
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();//关闭释放资源
            return builder.ToString();
        }


        /// <summary>
        /// WeiFos_Key 加密
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string GetEncryption(string strValue)
        {
            return GetEncryption(strValue, ConfigManage.AppSettings<string>("AppSettings:WeiFosKey"));
        }

        #endregion


        #region 解密字符串


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDecryption(string strValue, string key)
        {
            string strKey = StringHelper.ConvertTo32BitSHA1(key).Substring(0, 8);

            //解密标准算法的对象
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            //建立解密密对象的密钥和偏移量
            provider.Key = Encoding.ASCII.GetBytes(strKey);
            //原文使用Encoding.ASCII方法的GetBytes方法
            provider.IV = Encoding.ASCII.GetBytes(strKey);
            //将要解密的字符放到byte数组中
            byte[] buffer = new byte[strValue.Length / 2];
            //循环遍历遍历
            for (int i = 0; i < (strValue.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(strValue.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            //输入的文本必须是英文文本
            MemoryStream stream = new MemoryStream();
            //定义将数据连接到解密转换的流
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            //将当前字节写入到流中
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();//清除缓存区
            stream.Close();//关闭释放资源 
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string GetDecryption(string strValue)
        {
            return GetDecryption(strValue, ConfigManage.AppSettings<string>("AppSettings:WeiFosKey"));
        }

        #endregion


        /// <summary>
        /// 提取字符串金额
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal GetStrMoney(string str)
        {
            string str1 = string.Empty;
            Regex r = new Regex(@"([0-9]\d*\.?\d*)|(0\.\d*[0-9])");
            Match m = r.Match(str);
            while (m.Success)
            {
                str1 += m.Groups[0].Value;
                m = m.NextMatch();
            }

            return Convert.ToDecimal(str1);
        }

        /// <summary>
        /// 提取字符串金额
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStrDatetime(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            Regex reg = new Regex(@"((?<!\d)((\d{2,4}(\.|年|\/|\-))((((0?[13578]|1[02])(\.|月|\/|\-))((3[01])|([12][0-9])|(0?[1-9])))|(0?2(\.|月|\/|\-)((2[0-8])|(1[0-9])|(0?[1-9])))|(((0?[469]|11)(\.|月|\/|\-))((30)|([12][0-9])|(0?[1-9]))))|((([0-9]{2})((0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))(\.|年|\/|\-))0?2(\.|月|\/|\-)29))日?(?!\d))");
            string str1 = "";
            Match m = reg.Match(str);
            while (m.Success)
            {
                str1 += m.Groups[0].Value;
                m = m.NextMatch();
            }

            return str1;
        }


        /// <summary>
        /// 动态对象转换为实体对象支持泛型
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T DynToEntity<T>(dynamic o)
        {
            string json = JsonConvert.SerializeObject(o);
            return JsonConvert.DeserializeObject<T>(json);
        }


        /// <summary>
        /// 返回数组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ArrayList DataTableToObj(DataTable dt)
        {
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
            return arrayList;
        }

        /// <summary>
        /// DataRow转匿名对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static dynamic DataRowToObj(DataRow dr)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                dictionary.Add(dr.Table.Columns[i].ColumnName, dr[dr.Table.Columns[i].ColumnName].ToString());
            }
            return dictionary;
        }


        public static void SendSMS(string message, string serviceurl)
        {
            string sname = "";
            string spwd = "";
            string scorpid = "";
            string sprdid = "";
            string sdst = "";

            string postStrTpl = "sname={0}&spwd={1}&scorpid={2}&sprdid={3}&sdst={4}&smsg={5}";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, sname, spwd, scorpid, sprdid, sdst, message));

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceurl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                //反序列化upfileMmsMsg.Text
                //实现自己的逻辑
            }
            else
            {
                //访问失败
            }
        }


        /// <summary>
        /// 自定义属性排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string[] SortProperty<T>(T t)
        {
            return SortProperty<T>(t, false);
        }


        /// <summary>
        /// 自定义属性排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="isnull">是否序列化空字段</param>
        /// <returns></returns>
        public static string[] SortProperty<T>(T t, bool isnull)
        {
            Dictionary<string, string> bizObj = new Dictionary<string, string>();
            PropertyInfo[] propertys = t.GetType().GetProperties();

            foreach (PropertyInfo p in propertys)
            {
                string fieldName = p.Name;
                object val = p.GetValue(t, null);
                if (!isnull)
                {
                    if (val != null && val.ToString() != "")
                    {
                        bizObj.Add(p.Name, val.ToString());
                    }
                }
                else
                {
                    bizObj.Add(p.Name, val.ToString());
                }
            }

            var buff = new List<string>();
            var values = new List<string>();
            var result = from pair in bizObj orderby pair.Key select pair;
            foreach (var o in result)
            {
                if (string.IsNullOrEmpty(o.Key.Trim())) continue;
                buff.Add(string.Format("{0}={1}", o.Key.Trim(), o.Value.Trim()));
                values.Add(o.Value.Trim());
            }

            string signStr = string.Join("|", values.ToArray());

            string[] tmp = new string[2];
            tmp[0] = signStr;
            tmp[1] = string.Join("&", buff.ToArray());

            return tmp;
        }


        /// <summary>
        /// Json 格式序列化排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SortPropertyToJson<T>(T t)
        {
            return SortPropertyToJson<T>(t, true);
        }

        /// <summary>
        /// Json 格式序列化排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="isasc"></param>
        /// <returns></returns>
        public static string SortPropertyToJson<T>(T t, bool isasc)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            Dictionary<string, object> bizObj = new Dictionary<string, object>();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            ArrayList arrayList = new ArrayList();

            foreach (PropertyInfo p in propertys)
            {
                string fieldName = p.Name;
                object val = p.GetValue(t, null);
                bizObj.Add(p.Name, val);
            }

            if (isasc)
            {
                var result = from pair in bizObj orderby pair.Key select pair;
                foreach (var l in result)
                {
                    dictionary.Add(l.Key, l.Value);
                }
                return JsonConvert.SerializeObject(dictionary);
            }
            else
            {
                var result = from pair in bizObj orderby pair.Key descending select pair;
                foreach (var l in result)
                {
                    dictionary.Add(l.Key, l.Value);
                }
                return JsonConvert.SerializeObject(dictionary);
            }
        }

         
        #region  RSA客户端js加密服务器

        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

        public static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        public static string BytesToDecString(byte[] input)
        {
            StringBuilder decString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                decString.Append(String.Format(i == 0 ? "{0:D3}" : "-{0:D3}", input[i]));
            }
            return decString.ToString();
        }

        // Bytes are string
        public static string ASCIIBytesToString(byte[] input)
        {
            System.Text.ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(input);
        }

        public static string UTF16BytesToString(byte[] input)
        {
            System.Text.UnicodeEncoding enc = new UnicodeEncoding();
            return enc.GetString(input);
        }

        public static string UTF8BytesToString(byte[] input)
        {
            System.Text.UTF8Encoding enc = new UTF8Encoding();
            return enc.GetString(input);
        }

        // Base64
        public static string ToBase64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] FromBase64(string base64)
        {
            return Convert.FromBase64String(base64);
        }


        #endregion


        #region WebAPI AES加密 @Add:wangyj

        //密钥
        public static string key = "1234abcdABCD!@#$";

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }


        #endregion

    }
}