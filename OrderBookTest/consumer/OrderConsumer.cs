using System.Collections.Generic;
using System.Linq;
using CMCMarkets.Prophet.OrderBookTest.Model;
using System;
using CMCMarkets.Prophet.OrderBookTest.api;

namespace CMCMarkets.Prophet.OrderBookTest.consumer
{


    /// <summary>
    /// Fill in this class and any other required classes.
    /// </summary>
    class OrderConsumer : IOrderConsumer
    {
        private ILog _logger;

        private readonly List<OrderBook> _orderBooks = new List<OrderBook>();
        private readonly List<Order> _orders = new List<Order>();
        private readonly object _locker = new object();

        public void StartProcessing(object sender, ProcessingStartEventArgs args)
        {
            _logger = args.Log;
            _logger.log(LogLevel.INFO, "Processing Started!");
        }

        public void HandleOrderAction(object sender, OrderActionEventArgs args)
        {
            lock (_locker)
            {
                var order = args.order;

                switch (args.action)
                {
                    case api.Action.ADD:
                        {
                            _orders.Add(args.order);
                            var existingOrderBook = GetExistinOrderBook(order.symbol, order.price, order.isBuy);

                            if (existingOrderBook != null)
                            {
                                IncreaseOrderBookCount(existingOrderBook, order);
                            }
                            else
                            {
                                CreateNewOrderBook(order.isBuy, order.price, order.quantity, order.symbol);
                            }
                            break;
                        }

                    case api.Action.EDIT:
                        {
                            var previousOrder = _orders.SingleOrDefault(o => o.orderId == order.orderId);
                            if (previousOrder != null)
                            {
                                var existingOrderBook = GetExistinOrderBook(previousOrder.symbol, order.price, previousOrder.isBuy);
                                if (existingOrderBook == null)
                                {
                                    CreateNewOrderBook(previousOrder.isBuy, order.price, order.quantity, previousOrder.symbol);
                                }
                                else
                                {
                                    IncreaseOrderBookCount(existingOrderBook, order);
                                }
                            }
                            break;
                        }

                    case api.Action.REMOVE:
                        {
                            var previousOrder = _orders.SingleOrDefault(o => o.orderId == order.orderId);
                            if (previousOrder != null)
                            {
                                var existingOrderBook = GetExistinOrderBook(previousOrder.symbol, previousOrder.price, previousOrder.isBuy);
                                if (existingOrderBook != null)
                                {
                                    existingOrderBook.Size -= previousOrder.quantity;
                                    existingOrderBook.OrderCount -= 1;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private OrderBook GetExistinOrderBook(string symbol, int price, bool isBuy)
        {
            return _orderBooks.SingleOrDefault(o => o.Symbol == symbol && o.Price == price && o.IsBuy == isBuy);
        }

        private void CreateNewOrderBook(bool isBuy, int price, int quantity, string symbol)
        {
            _orderBooks.Add(new OrderBook
            {
                IsBuy = isBuy,
                OrderCount = 1,
                Price = price,
                Size = quantity,
                Symbol = symbol
            });
        }

        private static void IncreaseOrderBookCount(OrderBook existingOrderBook, Order order)
        {
            existingOrderBook.Size += order.quantity;
            existingOrderBook.OrderCount += 1;
        }

        public void FinishProcessing(object sender, EventArgs args)
        {
            var groupedOrderBooks = _orderBooks.GroupBy(x => new { x.Symbol, x.IsBuy });

            foreach (var groupedOrderBook in groupedOrderBooks)
            {
                _logger.log(LogLevel.INFO, string.Format("{0} - {1}", groupedOrderBook.Key.Symbol, groupedOrderBook.Key.IsBuy ? "Buy" : "Sell"));
                foreach (var orderBook in groupedOrderBook.Where(g => g.Size != 0))
                {
                    _logger.log(LogLevel.INFO, string.Format("{0} - {1} - {2}", orderBook.Price, orderBook.Size, orderBook.OrderCount));
                }
            }

        }
    }
}
