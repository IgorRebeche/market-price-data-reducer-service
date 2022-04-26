namespace Application.Services.Ticker
{
    public class TickerResponse
    {
        public string BrokerName { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public long Timestamp { get; set; }
    }
}