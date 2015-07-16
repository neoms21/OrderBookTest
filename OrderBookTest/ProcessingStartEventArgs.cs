namespace CMCMarkets.Prophet.OrderBookTest
{
    using System;
    using CMCMarkets.Prophet.OrderBookTest.api;

    class ProcessingStartEventArgs : EventArgs
    {
        public ILog Log { get; private set; }

        public ProcessingStartEventArgs(ILog log) { this.Log = log; }
    }
}
