﻿namespace CMCMarkets.Prophet.OrderBookTest
{
    using CMCMarkets.Prophet.OrderBookTest.consumer;
    using CMCMarkets.Prophet.OrderBookTest.api;

    class Program
    {
        static void Main(string[] args)
        {
            var environment = new AppEnvironmentImpl(LogLevel.INFO);
            var consumer = new OrderConsumer();
            
            environment.ProcessingStartEvent += consumer.StartProcessing;
            environment.ProcessingFinishEvent += consumer.FinishProcessing;
            environment.OrderActionEvent += consumer.HandleOrderAction;

            environment.Run();
        }
    }
}
