﻿using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace WebStore.Logger
{
    public static class Log4NetLoggerFactoryExtesions
    {
        private static string CheckFilePath(string filePath)
        {
            //if (filePath is null || filePath.Length == 0)
            if (filePath is not { Length: > 0 })
                throw new ArgumentException("Не указан путь к файлу");

            if (Path.IsPathRooted(filePath))
                return filePath;

            var assembly = Assembly.GetEntryAssembly();
            var dir = Path.GetDirectoryName(assembly!.Location);
            return Path.Combine(dir!, filePath);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configurationFile = "log4net.config")
        {
            factory.AddProvider(new Log4NetLoggerProvider(CheckFilePath(configurationFile)));
            return factory;
        }
    }
}
