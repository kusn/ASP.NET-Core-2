using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _СonfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

        public Log4NetLoggerProvider(string configurationFile)
        {
            _СonfigurationFile = configurationFile;
        }

        public ILogger CreateLogger(string category) =>
            _Loggers.GetOrAdd(category, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_СonfigurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });

        public void Dispose()
        {
            _Loggers.Clear();
        }
    }
}
