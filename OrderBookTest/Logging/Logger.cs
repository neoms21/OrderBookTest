using System.IO;
using CMCMarkets.Prophet.OrderBookTest.api;

namespace CMCMarkets.Prophet.OrderBookTest.Logging
{
    public class Logger : ILog
    {
        private const string LogFilePath = "Log.log";

        public void log(LogLevel level, string msg)
        {
            using (var fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(msg);
            }
        }
    }
}