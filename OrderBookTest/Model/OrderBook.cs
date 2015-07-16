namespace CMCMarkets.Prophet.OrderBookTest.Model
{
    public class OrderBook
    {
        public int Price { get; set; }
        public string Symbol { get; set; }
        public int Size { get; set; }
        public int OrderCount { get; set; }
        public bool IsBuy { get; set; }
    }
}