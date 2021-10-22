using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml;

namespace WebStore.Logger
{

    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string category, XmlElement configuration)
        {
            var logger_repository = LogManager
                .CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _Log = LogManager.GetLogger(logger_repository.Name, category);

            log4net.Config.XmlConfigurator.Configure(configuration);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        //public bool IsEnabled(LogLevel level)
        //{
        //    switch (level)
        //    {
        //        default:
        //            throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(LogLevel));

        //        case LogLevel.None:
        //            return false;

        //        case LogLevel.Trace:
        //            return _Log.IsDebugEnabled;

        //        case LogLevel.Debug:
        //            return _Log.IsDebugEnabled;

        //        case LogLevel.Information:
        //            return _Log.IsInfoEnabled;

        //        case LogLevel.Warning:
        //            return _Log.IsWarnEnabled;

        //        case LogLevel.Error:
        //            return _Log.IsErrorEnabled;

        //        case LogLevel.Critical:
        //            return _Log.IsFatalEnabled;
        //    }
        //}

        public bool IsEnabled(LogLevel level) => level switch
        {
            LogLevel.None => false,
            LogLevel.Trace => _Log.IsDebugEnabled,
            LogLevel.Debug => _Log.IsDebugEnabled,
            LogLevel.Information => _Log.IsInfoEnabled,
            LogLevel.Warning => _Log.IsWarnEnabled,
            LogLevel.Error => _Log.IsErrorEnabled,
            LogLevel.Critical => _Log.IsFatalEnabled,
            _ => throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(LogLevel)),
        };

        public void Log<TState>(
            LogLevel level,
            EventId id,
            TState state,
            Exception error,
            Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(level))
                return;

            var log_string = formatter(state, error);
            if (string.IsNullOrEmpty(log_string) && error is null)
                return;

            switch(level)
            { 
                default: 
                    throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(LogLevel));

                case LogLevel.None:
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(log_string);
                    break;

                case LogLevel.Information:
                    _Log.Info(log_string);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(log_string);
                    break;

                case LogLevel.Error:
                    _Log.Error(log_string, error);
                    break;

                case LogLevel.Critical:
                    _Log.Fatal(log_string, error);
                    break;
            }
        }
    }
}
