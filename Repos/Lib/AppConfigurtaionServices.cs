﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repos.Lib
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class AppConfigurtaionServices
    {
        public static IConfiguration Configuration { get; set; }
        static AppConfigurtaionServices()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载            
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }
    }
}
