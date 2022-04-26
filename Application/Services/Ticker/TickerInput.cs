namespace Application.Services.Ticker
{
    public class TickerInput
    {
        public string BrokerName { get; set; }
        public string Symbol { get; set; }
        public string TimeFrame { get; set; }
        public long TimeStampFrom { get; set; }
    }
}
