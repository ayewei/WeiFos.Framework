using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WeiFos.Core.JsonHelper;
using WeiFos.Core.Signature;
using Microsoft.CSharp.RuntimeBinder;

namespace WeiFos.Core
{
    /// <summary>
    /// 算法
    /// @author yewei 
    /// @date 2013-11-11
    /// </summary>
    public static class AlgorithmHelper
    {
        static object obj = new object();

        private static Random r = new Random();

        #region RSA公私钥对

        private static readonly string _privateKey = @"MIICXgIBAAKBgQC0xP5HcfThSQr43bAMoopbzcCyZWE0xfUeTA4Nx4PrXEfDvybJ
EIjbU/rgANAty1yp7g20J7+wVMPCusxftl/d0rPQiCLjeZ3HtlRKld+9htAZtHFZ
osV29h/hNE9JkxzGXstaSeXIUIWquMZQ8XyscIHhqoOmjXaCv58CSRAlAQIDAQAB
AoGBAJtDgCwZYv2FYVk0ABw6F6CWbuZLUVykks69AG0xasti7Xjh3AximUnZLefs
iuJqg2KpRzfv1CM+Cw5cp2GmIVvRqq0GlRZGxJ38AqH9oyUa2m3TojxWapY47zye
PYEjWwRTGlxUBkdujdcYj6/dojNkm4azsDXl9W5YaXiPfbgJAkEA4rlhSPXlohDk
FoyfX0v2OIdaTOcVpinv1jjbSzZ8KZACggjiNUVrSFV3Y4oWom93K5JLXf2mV0Sy
80mPR5jOdwJBAMwciAk8xyQKpMUGNhFX2jKboAYY1SJCfuUnyXHAPWeHp5xCL2UH
tjryJp/Vx8TgsFTGyWSyIE9R8hSup+32rkcCQBe+EAkC7yQ0np4Z5cql+sfarMMm
4+Z9t8b4N0a+EuyLTyfs5Dtt5JkzkggTeuFRyOoALPJP0K6M3CyMBHwb7WsCQQCi
TM2fCsUO06fRQu8bO1A1janhLz3K0DU24jw8RzCMckHE7pvhKhCtLn+n+MWwtzl/
L9JUT4+BgxeLepXtkolhAkEA2V7er7fnEuL0+kKIjmOm5F3kvMIDh9YC1JwLGSvu
1fnzxK34QwSdxgQRF1dfIKJw73lClQpHZfQxL/2XRG8IoA==".Replace("\n", "");

        //openssl rsa -pubout -in rsa_1024_priv.pem -out rsa_1024_pub.pem
        private static readonly string _publicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC0xP5HcfThSQr43bAMoopbzcCy
ZWE0xfUeTA4Nx4PrXEfDvybJEIjbU/rgANAty1yp7g20J7+wVMPCusxftl/d0rPQ
iCLjeZ3HtlRKld+9htAZtHFZosV29h/hNE9JkxzGXstaSeXIUIWquMZQ8XyscIHh
qoOmjXaCv58CSRAlAQIDAQAB".Replace("\n", "");

        #endregion

        #region 概率算法
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static long GetRandomNumber(this Random random, long min, long max)
        {
            byte[] minArr = BitConverter.GetBytes(min);

            int hMin = BitConverter.ToInt32(minArr, 4);

            int lMin = BitConverter.ToInt32(new byte[] { minArr[0], minArr[1], minArr[2], minArr[3] }, 0);

            byte[] maxArr = BitConverter.GetBytes(max);

            int hMax = BitConverter.ToInt32(maxArr, 4);

            int lMax = BitConverter.ToInt32(new byte[] { maxArr[0], maxArr[1], maxArr[2], maxArr[3] }, 0);

            if (random == null)
            {
                random = new Random();
            }

            int h = random.Next(hMin, hMax);

            int l = 0;

            if (h == hMin)
            {
                l = random.Next(Math.Min(lMin, lMax), Math.Max(lMin, lMax));
            }
            else
            {
                l = random.Next(0, Int32.MaxValue);
            }

            byte[] lArr = BitConverter.GetBytes(l);

            byte[] hArr = BitConverter.GetBytes(h);

            byte[] result = new byte[8];

            for (int i = 0; i < lArr.Length; i++)
            {
                result[i] = lArr[i];
                result[i + 4] = hArr[i];
            }

            return BitConverter.ToInt64(result, 0);
        }

        /// <summary>
        /// 获取概率的基数
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static long GetBaseNumber(double[] array)
        {
            long result = 0;

            try
            {
                if (array == null || array.Length == 0)
                {
                    return result;
                }

                string targetNumber = string.Empty;

                foreach (double item in array)
                {
                    string temp = item.ToString();

                    if (!temp.Contains('.'))
                    {
                        continue;
                    }

                    temp = temp.Substring(temp.IndexOf('.')).Replace(".", "");

                    if (targetNumber.Length < temp.Length)
                    {
                        targetNumber = temp;
                    }
                }

                if (!string.IsNullOrEmpty(targetNumber))
                {
                    int ep = targetNumber.Length;

                    result = (long)Math.Pow(10, ep);
                }
            }
            catch { }

            return result;
        }


        /// <summary>
        /// 概率算法
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="DList">对象对应索引</param>
        /// <returns></returns>
        public static T Probability<T>(Dictionary<T, double> DList)
        {
            T Result = default(T);

            if (DList == null || DList.Count == 0)
                return default(T);

            //求出概率基数
            long basicNumber = GetBaseNumber(DList.Select(k => k.Value).ToArray());

            //随机数
            long diceRoll = GetRandomNumber(new Random(), 1, basicNumber);

            //
            long cumulative = 0;

            foreach (var k in DList)
            {
                cumulative += (long)(k.Value * basicNumber);
                if (diceRoll <= cumulative)
                {
                    return k.Key;
                }
            }

            return Result;
        }
        #endregion

        #region 生成不重复的串号算法
        /// <summary>
        /// 生成不重复的串号，规则:前10位为日期,未4位从最大值max开始倒序
        /// </summary>
        /// <param name="qty">产生数量</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static string[] GeneratedNums(int qty, int min, int max, string prefix)
        {
            List<string> list = new List<string>();
            int k = 1;
            int seed = new Random().Next(min, max);
            for (int i = seed; i > 0; i--)
            {
                list.Add(prefix + diyNumStr(max.ToString().Length, i));
                if (k == qty) break;
                k++;
            }
            //打乱
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return list.OrderBy(o => rand.Next()).ToArray();
        }

        /// <summary>
        /// 按位取数，不足补0
        /// </summary>
        /// <param name="countLen"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private static String diyNumStr(int countLen, int num)
        {
            int diyNum = countLen - num.ToString().Length;
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < diyNum; i++)
            {
                result.Append("0");
            }
            return result.ToString() + num.ToString();
        }


        /// <summary>
        /// 使用Random生一个字符串，记录到数组中，再生成一个如果不在数组中则插入，效率极低
        /// 采用 第一个随机数在0-X之间。第二个在X-X*2之间，
        /// </summary>
        /// <param name="len"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<string> GetRandString(int len, int count)
        {
            double max_value = Math.Pow(36, len);
            if (max_value > long.MaxValue) return null;

            long all_count = (long)max_value;
            long stepLong = all_count / count;
            if (stepLong > int.MaxValue) return null;

            int step = (int)stepLong;
            if (step < 3) return null;

            long begin = 0;
            List<string> list = new List<string>();
            Random rand = new Random();
            while (true)
            {
                long value = rand.Next(1, step) + begin;
                begin += step;
                list.Add(GetChart(len, value));
                if (list.Count == count)
                {
                    break;
                }
            }

            list = SortByRandom(list);

            return list;
        }


        /// <summary>
        /// 将数字转化成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetChart(int len, long value)
        {
            const string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder str = new StringBuilder();
            while (true)
            {
                str.Append(CHAR[(int)(value % 36)]);
                value = value / 36;
                if (str.Length == len)
                {
                    break;
                }
            }

            return str.ToString();
        }


        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="charList"></param>
        /// <returns></returns>
        private static List<string> SortByRandom(List<string> charList)
        {
            Random rand = new Random();
            for (int i = 0; i < charList.Count; i++)
            {
                int index = rand.Next(0, charList.Count);
                string temp = charList[i];
                charList[i] = charList[index];
                charList[index] = temp;
            }

            return charList;
        }

        #endregion



        #region 计算地图两点之间的距离

        /// <summary>
        /// 短距离计算，利用勾股定理计算，适用于两点距离很近的情况；
        /// </summary>
        /// <param name="sLat"></param>
        /// <param name="sLng"></param>
        /// <param name="eLat"></param>
        /// <param name="eLng"></param>
        /// <returns></returns>
        private static double GetRealDistance(double sLat, double sLng, double eLat, double eLng)
        {
            double distance = Math.Round(
                6378.138 * 2 * Math.Asin(
                    Math.Sqrt(
                        Math.Pow(Math.Sin((sLat * Math.PI / 180 - eLat * Math.PI / 180)) / 2, 2) +
                        Math.Cos(sLat * Math.PI / 180) * Math.Cos(eLat * Math.PI / 180) *
                        Math.Pow(Math.Sin((sLng * Math.PI / 180 - eLng * Math.PI / 180)) / 2, 2)
                    )
                ) * 1000
            );
            return distance;
        }

        /// <summary>
        /// 短距离计算，利用勾股定理计算，适用于两点距离很近的情况；
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double GetShortDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double DEF_PI = 3.14159265359; // PI
            double DEF_2PI = 6.28318530712; // 2*PI
            double DEF_PI180 = 0.01745329252; // PI/180.0
            double DEF_R = 6370693.5; // radius of earth

            double ew1, ns1, ew2, ns2;
            double dx, dy, dew;
            double distance;
            // 角度转换为弧度
            ew1 = lon1 * DEF_PI180;
            ns1 = lat1 * DEF_PI180;
            ew2 = lon2 * DEF_PI180;
            ns2 = lat2 * DEF_PI180;
            // 经度差
            dew = ew1 - ew2;
            // 若跨东经和西经180 度，进行调整
            if (dew > DEF_PI)
                dew = DEF_2PI - dew;
            else if (dew < -DEF_PI)
                dew = DEF_2PI + dew;
            dx = DEF_R * Math.Cos(ns1) * dew; // 东西方向长度(在纬度圈上的投影长度)
            dy = DEF_R * (ns1 - ns2); // 南北方向长度(在经度圈上的投影长度)
            // 勾股定理求斜边长
            distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        /// <summary>
        /// 长距离计算，按标准的球面大圆劣弧长度计算，适用于距离较远的情况；
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double GetLongDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double DEF_PI180 = 0.01745329252; // PI/180.0
            double DEF_R = 6370693.5; // radius of earth
            double ew1, ns1, ew2, ns2;
            double distance;
            // 角度转换为弧度
            ew1 = lon1 * DEF_PI180;
            ns1 = lat1 * DEF_PI180;
            ew2 = lon2 * DEF_PI180;
            ns2 = lat2 * DEF_PI180;
            // 求大圆劣弧与球心所夹的角(弧度)
            distance = Math.Sin(ns1) * Math.Sin(ns2) + Math.Cos(ns1) * Math.Cos(ns2) * Math.Cos(ew1 - ew2);
            // 调整到[-1..1]范围内，避免溢出
            if (distance > 1.0)
                distance = 1.0;
            else if (distance < -1.0)
                distance = -1.0;
            // 求大圆劣弧长度
            distance = DEF_R * Math.Acos(distance);
            return distance;
        }
        #endregion

        #region 根据当前时间创建编号
        /// <summary>
        /// 该编号由年、月、日时间的字符串构成
        /// </summary>
        /// <returns></returns>
        public static string CreateSerialNumber()
        {
            DateTime now = DateTime.Now;
            ///构建字符串
            string fileName = now.Year.ToString()
            + (now.Month.ToString().Length == 1 ? "0" + now.Month.ToString() : now.Month.ToString())
            + (now.Day.ToString().Length == 1 ? "0" + now.Day.ToString() : now.Day.ToString());
            return fileName;
        }
        #endregion

        #region 生成订单号
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            return CreateNo("");
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="prdfix"></param>
        /// <returns></returns>
        public static string CreateNo(string prdfix)
        {
            string result = "";
            int random = new Random().Next(1000000);
            lock (obj)
            {
                result = prdfix + DateTime.Now.ToString("yyMMddHHmmss") + random.ToString("D6");
            }
            return result;
        }

        #endregion

        #region 生成固定标识订单号16

        /// <summary>
        /// 生成固定标识订单号16
        /// </summary>
        /// <param name="firstNum">订单号首数字</param>
        /// <returns></returns>
        public static string CreateNo16()
        {
            string result = "";
            int random = new Random().Next(10000);
            lock (obj)
            {
                result = DateTime.Now.ToString("yyMMddHHmmss") + random.ToString("D4");
            }
            return result;
        }

        #endregion


        #region RSA加密算法


        #region Bouncy Castle 方式


        /// <summary>
        /// 生成公私钥对
        /// </summary>
        /// <param name="strength"></param>
        /// <returns></returns>
        public static (string, string) CreateKeyPair(int strength = 1024)
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            AsymmetricCipherKeyPair keys = r.GenerateKeyPair();

            TextWriter privateTextWriter = new StringWriter();
            PemWriter privatePemWriter = new PemWriter(privateTextWriter);
            privatePemWriter.WriteObject(keys.Private);
            privatePemWriter.Writer.Flush();


            TextWriter publicTextWriter = new StringWriter();
            PemWriter publicPemWriter = new PemWriter(publicTextWriter);
            publicPemWriter.WriteObject(keys.Public);
            publicPemWriter.Writer.Flush();


            return (publicTextWriter.ToString(), privateTextWriter.ToString());
        }


        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="decryptstring">待解密的字符串(Base64)</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string privateKey, string decryptstring)
        {
            //正确格式-----BEGIN RSA PRIVATE KEY-----\r\nXXXXXXXX\r\n-----END RSA PRIVATE KEY-----
            if (privateKey.IndexOf("-----BEGIN RSA PRIVATE KEY-----\r\n") == -1) privateKey = privateKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "-----BEGIN RSA PRIVATE KEY-----\r\n");
            if (privateKey.IndexOf("\r\n-----END RSA PRIVATE KEY-----") == -1) privateKey = privateKey.Replace("-----END RSA PRIVATE KEY-----", "\r\n-----END RSA PRIVATE KEY-----");

            using (TextReader reader = new StringReader(privateKey))
            {
                dynamic key = new PemReader(reader).ReadObject();

                //与前端JsEncrypt交互,要使用Pkcs1Encoding
                var rsaDecrypt = new Pkcs1Encoding(new RsaEngine());
                if (key is AsymmetricKeyParameter)
                {
                    key = (AsymmetricKeyParameter)key;
                }
                else if (key is AsymmetricCipherKeyPair)
                {
                    key = ((AsymmetricCipherKeyPair)key).Private;
                }
                rsaDecrypt.Init(false, key);  //这里加密是true；解密是false  

                byte[] entData = Convert.FromBase64String(decryptstring);
                entData = rsaDecrypt.ProcessBlock(entData, 0, entData.Length);
                return Encoding.UTF8.GetString(entData);
            }
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptstring">待加密的字符串</param>
        /// <returns>加密后的Base64</returns>
        public static string Encrypt(string publicKey, string encryptstring)
        {
            using (TextReader reader = new StringReader(publicKey))
            {
                AsymmetricKeyParameter key = new PemReader(reader).ReadObject() as AsymmetricKeyParameter;
                Pkcs1Encoding pkcs1 = new Pkcs1Encoding(new RsaEngine());
                pkcs1.Init(true, key);//加密是true；解密是false;
                byte[] entData = Encoding.UTF8.GetBytes(encryptstring);
                entData = pkcs1.ProcessBlock(entData, 0, entData.Length);
                return Convert.ToBase64String(entData);
            }
        }

        #endregion



        /// <summary>
        /// 生成私钥
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static RSA CreateRsaFromPrivateKey(string privateKey = null)
        {
            if (string.IsNullOrEmpty(privateKey)) privateKey = _privateKey;

            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            var rsa = RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }


        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }


        /// <summary>
        /// 生成公钥
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static RSA CreateRsaFromPublicKey(string publicKey = null)
        {
            if (string.IsNullOrEmpty(publicKey)) publicKey = _publicKey;

            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;

            x509key = Convert.FromBase64String(publicKey);
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return null;
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }

            }
        }


        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        #endregion



        /// <summary>
        /// 生产至少15位长度
        /// </summary>
        /// <param name="prdfix"></param>
        /// <returns></returns>
        public static string CreateNo16(string prdfix)
        {
            var dtString = DateTime.Now.ToString("yyMMddHHmmss");
            var rdString = GetRandomNum(3);
            return string.Format("{0}{1}{2}", prdfix, dtString, rdString);
        }


        /// <summary>
        /// 生成至少19位长度
        /// </summary>
        /// <param name="prdfix">前缀</param>
        /// <returns></returns>
        public static string CreateNo19(string prdfix)
        {
            var dtString = DateTime.Now.ToString("yyMMddHHmmssff");
            var rdString = GetRandomNum(5);
            return string.Format("{0}{1}{2}", prdfix, dtString, rdString);
        }


        /// <summary>
        /// 生成大于14位长度
        /// </summary>
        /// <param name="len">前缀</param>
        /// <returns></returns>
        public static string CreateNo(int len)
        {
            if (len < 15) return "";
            var dtString = DateTime.Now.ToString("yyMMddHHmmssff");
            var rdString = GetRandomNum(len - dtString.Length);
            return string.Format("{0}{1}", dtString, rdString);
        }


        /// <summary>
        /// 获取随机数（存数字）
        /// </summary>
        /// <param name="len">字串长度</param>
        /// <returns></returns>
        public static string GetRandomNum(int len)
        {
            var result = string.Empty;

            for (int i = 0; i < len; i++)
            {
                result += r.Next(0, 10);
            }
            return result;
        }


        /// <summary>
        /// 项目加密签名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPMValue(string key)
        {
            StringBuilder datas = new StringBuilder();
            datas.Append("{\"key\":\"").Append(key).Append("\"}");

            var data = new
            {
                key = key,
                value = StringHelper.ConvertTo32BitSHA1(datas.ToString())
            };

            //签名 数据包属性排序，序列化
            JObject obj = JObject.Parse(JsonConvert.SerializeObject(data));
            return SignHelper.SignMD5(JsonSort.SortJson(obj, null), "yewei888");
        }





    }

}
