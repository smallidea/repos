﻿using Microsoft.AspNetCore.Builder;
using System;

using System.IO;
using System.Reflection;

namespace Repos.Lib
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用log4net配置
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLog4net(this IApplicationBuilder app)
        {
            var logRepository = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            return app;
        }
    }
}
