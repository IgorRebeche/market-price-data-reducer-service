using Application.Services.Ticker;
using Refit;


namespace Infrastructure.Services.Ticker
{
    public interface ITickerApi
    {
        [Get("/api/ticker")]
        Task<ApiResponse<IEnumerable<TickerResponse>>> GetTickers(string brokerName, string symbol, string timeFrame, long timeStampFrom);
    }
}
