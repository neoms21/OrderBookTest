using System;
using CMCMarkets.Prophet.OrderBookTest.api;
using Action = CMCMarkets.Prophet.OrderBookTest.api.Action;

namespace CMCMarkets.Prophet.OrderBookTest
{
 
    class OrderActionEventArgs : EventArgs
    {
        public Action action { get; private set; }
        public Order order { get; private set; }

        public OrderActionEventArgs(Action action, Order order)
        {
            this.action = action;
            this.order = order;
        }
    }
}
