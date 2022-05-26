using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration.Binder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace WeiFos.Core.SettingModule
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：配置文件操作类
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public class ConfigManage
    {


        /// <summary>
        /// 根据节点获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T AppSettings<T>(string key)
        {
            string appsettings = "appsettings.json";
           
            //默认通过当前程序所运行的位置为准
            string root = AppDomain.CurrentDomain.BaseDirectory;
            //通过AppDomain获取的目录如果包含bin目录则判定为.net core 环境
            if (root.IndexOf("bin\\Debug") != -1) root = Directory.GetParent(appsettings).FullName;

            IConfiguration config = new ConfigurationBuilder().SetBasePath(root)
            .Add(new JsonConfigurationSource { Path = appsettings, Optional = false, ReloadOnChange = true }).Build();

            return config.GetSection(key).Get<T>();

            //T appconfig = new ServiceCollection().AddOptions().Configure<T>(config.GetSection(key))
            //.BuildServiceProvider().GetService<IOptions<T>>().Value;
            //return appconfig;
        }


        /// <summary>
        /// 动态运行时
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        //public static dynamic GetAppSettings(string key)
        //{
        //    DirectoryInfo Dir = Directory.GetParent(AppContext.BaseDirectory);
        //    string root = Dir.Parent.Parent.Parent.FullName; Directory.GetParent(AppContext.BaseDirectory)

        //    IConfiguration config = new ConfigurationBuilder().SetBasePath(root)
        //    .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true }).Build();

        //    string ss = config.GetValue;

        //    if (string.IsNullOrEmpty(key)) return JsonConvert.DeserializeObject<dynamic>(strData)[key];
        //    return JsonConvert.DeserializeObject<dynamic>(strData);
        //}





    }
}
