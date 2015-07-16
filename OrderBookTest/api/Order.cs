namespace CMCMarkets.Prophet.OrderBookTest.api
{
    using System;
    class Order
    {
        public Order(long orderId, String symbol, bool isBuy, int price, int quantity)
        {
            this.orderId = orderId;
            this.symbol = symbol;
            this.isBuy = isBuy;
            this.price = price;
            this.quantity = quantity;
        }

        public long orderId { get; private set; }
        public String symbol { get; private set; }
        public bool isBuy { get; private set; }
        public int price { get; private set; }
        public int quantity { get; private set; }
    }
}
