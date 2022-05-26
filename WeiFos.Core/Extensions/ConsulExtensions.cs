using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.Core.Extensions
{
    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2022.03.04
    /// 描 述：Consul服务注册扩展
    /// </summary>
    public static class ConsulExtensions
    {

        public static void ConsulRegister(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dcl";
            });

            string ip = string.IsNullOrWhiteSpace(configuration["ip"]) ? "192.168.3.230" : configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);

            var result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                Address = "http://192.168.56.1:1234/Home/About",
                ID = "jw",
                Name = "jw",
                Port = 1234,
                Tags = new[] { "jiewus" },
                Check = new AgentServiceCheck()
                {
                    HTTP = "http://192.168.56.1:1234/Home/About",
                    Interval = new TimeSpan(0, 0, 10),
                    DeregisterCriticalServiceAfter = new TimeSpan(0, 1, 0),
                }
            }).Result;

        }


    }
}
