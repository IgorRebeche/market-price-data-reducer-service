namespace Application.Services.Ticker
{
    public interface ITickerService
    {
        public Task<IEnumerable<TickerResponse>> GetTickers(TickerInput tickerInput);
    }
}
