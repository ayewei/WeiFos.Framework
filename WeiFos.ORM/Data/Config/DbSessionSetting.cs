using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WeiFos.Core;
using WeiFos.Core.JsonHelper;
using WeiFos.Core.SettingModule;
using WeiFos.Core.Signature;
using WeiFos.ORM.Data.Const;

namespace WeiFos.ORM.Data.Config
{
    /// <summary>
    /// 数据库访问绘话 配置类
    /// @author yewei
    /// @date 2014-02-14
    /// </summary>
    public class DbSessionSetting
    {

        #region 单例模式

        private static DbSessionSetting instance = new DbSessionSetting();

        public static DbSessionSetting Instance
        {
            get { return instance; }
        }

        #endregion

        /// <summary>
        /// 定时器
        /// </summary>
        private static Timer timer;

        /// <summary>
        /// 是否检验
        /// </summary>
        private static bool is_check = false;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DbSessionSetting()
        {
            CheckAuth();

            timer = new Timer(1000 * 60 * 30);
            timer.Elapsed += new ElapsedEventHandler(TimesUp);
            //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            timer.AutoReset = true;
            timer.Start();
        }


        /// <summary>
        /// 获取当前访问权限
        /// </summary>
        /// <returns></returns>
        public bool Get()
        {
            return is_check;
        }


        /// <summary>
        /// 定时执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimesUp(object sender, ElapsedEventArgs e)
        {
            is_check = false;
            CheckAuth();
        }


        /// <summary>
        /// 执行授权
        /// </summary>
        private static void CheckAuth()
        {
            string key = "", val = "", pkey = "";

            //项目秘钥
            pkey = ConfigManage.AppSettings<string>("AppSettings:PKey");
            if (!string.IsNullOrEmpty(pkey))
            {
                if (!is_check)
                {
                    if (pkey.IndexOf("#") != -1)
                    {
                        key = pkey.Split('#')[0];
                        val = pkey.Split('#')[1];

                        string pval = AlgorithmHelper.GetPMValue(key);
                        if (pval.Equals(val))
                        {
                            is_check = true;
                        }
                    }
                    else
                    {
                        //数据包
                        StringBuilder query = new StringBuilder();
                        query.Append("{\"key\":\"").Append(pkey).Append("\"}");

                        //签名 数据包属性排序，序列化
                        JObject obj = JObject.Parse(query.ToString());

                        //将数据包签名
                        ORMSignPackage signPackage = new ORMSignPackage();
                        signPackage.Sign = SignHelper.SignMD5(JsonSort.SortJson(obj, null), "888yewei888");

                        StringBuilder sb = new StringBuilder();
                        sb.Append("{");
                        sb.Append("\"query\":").Append(query);
                        sb.Append(",\"global\":").Append(JsonConvert.SerializeObject(signPackage));
                        sb.Append("}");

                        string restut = HttpHelper.Post("http://pm.weifos.com/auth/PApi/100", sb.ToString());
                        string code = JsonConvert.DeserializeObject<dynamic>(restut).Code;
                        if (!string.IsNullOrEmpty(code) && "200".Equals(code))
                        {
                            is_check = true;
                        }
                        else
                        {
                            is_check = false;
                        }
                    }
                }

            }
            else
            {
                is_check = false;
            }
        }





    }
}
