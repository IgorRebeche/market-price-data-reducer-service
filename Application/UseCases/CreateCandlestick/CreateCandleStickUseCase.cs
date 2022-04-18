using Application.Common;
using Domain.Options;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.CreateCandlestick
{
    public class CreateCandleStickUseCase : ICreateCandleStickUseCase
    {
        private IOptions<List<Timeframe>> _timeFrameOptions;
        private IOptions<List<Asset>> _assetOptions;
        private ILogger<CreateCandleStickUseCase> _logger;
        private ITickerRepository _tickerRepository;
        private ICandleRepository _candleRepository;

        public CreateCandleStickUseCase(
            ILogger<CreateCandleStickUseCase> logger, 
            IOptions<List<Timeframe>> timeFrameOptions,
            IOptions<List<Asset>> assetOptions,
            ITickerRepository tickerRepository, 
            ICandleRepository candleRepository
        )
        {
            _timeFrameOptions = timeFrameOptions;
            _assetOptions = assetOptions;
            _logger = logger;
            _tickerRepository = tickerRepository;
            _candleRepository = candleRepository;
        }
        public async Task ExecuteAsync()
        {

            // TODO: Pegar a ultima candle registrado para o timeframe

            // TODO: Persistir cotações de um range de preço RAW

            // TODO: Criar candles disponiveis no range
            _assetOptions.Value.ForEach(asset =>
            {
                _timeFrameOptions.Value.ForEach(async timeFrame =>
                {
                    //var lastCandle = await _candleRepository.GetLastCandle(asset.BrokerName, asset.AssetName, timeFrame.TimeframeName);
                    long lastTimeStamp = 1649717930297;
                    
                    //if (lastCandle != null) lastTimeStamp = lastCandle.TimeStamp;
                    
                    var tickers = await _tickerRepository.GetTickersRange(asset.BrokerName, asset.AssetName, timeFrame.TimeframeName, lastTimeStamp, DateTimeOffset.Now.ToUnixTimeMilliseconds());
                    _logger.LogInformation("Tickers {@tickers}", tickers.Count());

                    _logger.LogInformation("Timeframe Name: {TimeframeName} | Timeframe Time in Miliseconds: {timeframeInMilli}",
                     timeFrame.TimeframeName, timeFrame.TimeframeInSeconds);
                    var helper = new CandleGenerationHelper();
                    var candles = helper.ProcessCandles(timeFrame, tickers.ToList());
                    candles.ForEach(async candle =>
                    {
                        //await _candleRepository.InsertCandle(candle);
                        _logger.LogInformation("Candle Inserted!: {@candle}", candle);
                    });
                });
            });
            
        }
    }
}