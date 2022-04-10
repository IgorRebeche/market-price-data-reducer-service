using Application.UseCases.CreateCandlestickUseCase;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.CreateCandlestick
{
    public class CreateCandleStickUseCase : ICreateCandleStickUseCase
    {
        private IOptions<List<Timeframe>> _timeFrameOptions;
        private ILogger<CreateCandleStickUseCase> _logger;

        public CreateCandleStickUseCase(ILogger<CreateCandleStickUseCase> logger, IOptions<List<Timeframe>> timeFrameOptions)
        {
            _timeFrameOptions = timeFrameOptions;
            _logger = logger;
        }
        public Task ExecuteAsync()
        {

            // TODO: Pegar a ultima candle registrado para o timeframe

            // TODO: Persistir cotações de um range de preço RAW

            // TODO: Criar candles disponiveis no range

            _timeFrameOptions.Value.ForEach(async x =>
            {
                _logger.LogInformation("Timeframe Name: {TimeframeName} | Timeframe Time in Miliseconds: {timeframeInMilli}",
                 x.TimeframeName, x.TimeframeInSeconds);

                //processCandles(x, );
            });
            return Task.CompletedTask;
        }

        private Task processCandles(Timeframe timeframe, Ticker[] tickers)
        {

            int temporaryTimebox = 0;
            List<Candle> candles = new List<Candle>();

            foreach (var ticker in tickers)
            {
                int actualCandle = candles.Count() - 1;
                // Pegar tickers dentro do timebox
                if (timeframe.TimeframeInSeconds == temporaryTimebox)
                {
                    candles.Add(new Candle());
                    candles[actualCandle].OpenPrice = ticker.Price;
                    actualCandle = candles.Count() - 1;
                    temporaryTimebox = 0;
                }

                if (candles[actualCandle].HighPrice < ticker.Price)
                {
                    candles[actualCandle].HighPrice = ticker.Price;
                }

                if (candles[actualCandle].LowPrice > ticker.Price)
                {
                    candles[actualCandle].LowPrice = ticker.Price;
                }

                temporaryTimebox++;
            }
            return Task.CompletedTask;
        }
    }
}