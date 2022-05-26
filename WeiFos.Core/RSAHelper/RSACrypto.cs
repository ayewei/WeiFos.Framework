using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WeiFos.Core.RSAHelper
{
    /// <summary>
    /// RSA加密类
    /// @author 叶委  
    /// add by @date 2015-02-14
    /// </summary>
    public class RSACrypto
    {

        private RSACryptoServiceProvider rsaCServiceProvider;
        public RSAParameters ExportParameters(bool includePrivateParameters)
        {
            return rsaCServiceProvider.ExportParameters(includePrivateParameters);
        }

        public void InitCrypto(string data)
        {
            //加密服务参数
            CspParameters cspParams = new CspParameters();

            //加密标示
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;

            rsaCServiceProvider = new RSACryptoServiceProvider(cspParams);
            rsaCServiceProvider.FromXmlString(data);
        }


        public byte[] Encrypt(string txt)
        {
            byte[] result;

            ASCIIEncoding enc = new ASCIIEncoding();
            int numOfChars = enc.GetByteCount(txt);
            byte[] tempArray = enc.GetBytes(txt);
            result = rsaCServiceProvider.Encrypt(tempArray, false);

            return result;
        }

        /// <summary>
        /// 字节加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] txt)
        {
            byte[] result;

            result = rsaCServiceProvider.Decrypt(txt, false);

            return result;
        }


    }

}