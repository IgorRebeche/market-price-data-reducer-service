using Application.Services.Ticker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Ticker
{
    public class TickerService : ITickerService
    {
        private ILogger<TickerService> _logger;
        private ITickerApi _tickerApi;

        public TickerService(ILogger<TickerService> logger,ITickerApi tickerApi)
        {
            _logger = logger;
            _tickerApi = tickerApi;
        }
        public async Task<IEnumerable<TickerResponse>> GetTickers(TickerInput tickerInput)
        {
            var tickersResponse = await _tickerApi.GetTickers(tickerInput.BrokerName, tickerInput.Symbol, tickerInput.TimeFrame, tickerInput.TimeStampFrom);
            if (!tickersResponse.IsSuccessStatusCode)
            {
                _logger.LogError("[{ClassName}] {Api} returned with error. Response: {response}", nameof(TickerService), nameof(ITickerApi), tickersResponse);
                throw new Exception("Error on getting Tickers from TickerApi");
            }
            return tickersResponse.Content;
        }
    }
}
