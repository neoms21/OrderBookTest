using System.IO;
using CMCMarkets.Prophet.OrderBookTest.api;

namespace CMCMarkets.Prophet.OrderBookTest.Logging
{

    public class Logger : ILog
    {
        private const string LogFilePath = "Log.log";

        public Logger()
        {
            //if (!File.Exists(LogFilePath))
            //    File.Create(LogFilePath);

        }
        public void log(LogLevel level, string msg)
        {
            using (FileStream fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(msg);
            }
        }
    }
}