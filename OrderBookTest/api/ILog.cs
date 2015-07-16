using System;

namespace CMCMarkets.Prophet.OrderBookTest.api
{
    interface ILog
    {
        void log(LogLevel level, String msg);
    }
}
