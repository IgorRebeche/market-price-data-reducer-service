using Application.Common;
using Application.Services.Ticker;
using Domain.Options;
using Domain.Repositories;
using Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.CreateCandlestick
{
    public class CreateCandleStickUseCase : ICreateCandleStickUseCase
    {
        private IOptions<List<TimeframeOptions>> _timeFrameOptions;
        private IOptions<List<AssetOptions>> _assetOptions;
        private ILogger<CreateCandleStickUseCase> _logger;
        private ITickerService _tickerService;
        private ICandleRepository _candleRepository;
        private IBus _bus;

        public CreateCandleStickUseCase(
            ILogger<CreateCandleStickUseCase> logger, 
            IOptions<List<TimeframeOptions>> timeFrameOptions,
            IOptions<List<AssetOptions>> assetOptions,
            ITickerService tickerService, 
            ICandleRepository candleRepository,
            IBus bus
        )
        {
            _timeFrameOptions = timeFrameOptions;
            _assetOptions = assetOptions;
            _logger = logger;
            _tickerService = tickerService;
            _candleRepository = candleRepository;
            _bus = bus;
        }
        public async Task ExecuteAsync()
        {           
            _assetOptions.Value.ForEach(asset =>
            {
                _timeFrameOptions.Value.ForEach(async timeFrame =>
                {
                    var lastCandle = await _candleRepository.GetLastCandle(asset.BrokerName, asset.AssetName, timeFrame.TimeframeName);
                    long lastTimeStamp = 0;
                    
                    if (lastCandle != null) lastTimeStamp = lastCandle.TimeStamp;
                    
                    var tickerInput = new TickerInput 
                    { 
                        BrokerName = asset.BrokerName, 
                        Symbol = asset.AssetName, 
                        TimeFrame = timeFrame.TimeframeName, 
                        TimeStampFrom = lastTimeStamp 
                    };
                    var tickers = await _tickerService.GetTickers(tickerInput);
                    _logger.LogInformation("[{ClassName}] Found Tickers {@tickers}", nameof(CreateCandleStickUseCase), tickers.Count());

                    _logger.LogInformation("[{ClassName}] Timeframe Name: {TimeframeName} | Timeframe Time in Miliseconds: {timeframeInMilli}",
                     nameof(CreateCandleStickUseCase), timeFrame.TimeframeName, timeFrame.TimeframeInSeconds);
                    
                    var helper = new CandleGenerationHelper();
                    
                    var candles = helper.ProcessCandles(timeFrame, tickers.ToList());
                    if (candles.Count == 0)
                    {
                        _logger.LogInformation("[{ClassName}] Candles were not found to insert", nameof(CreateCandleStickUseCase));
                        return;
                    }

                    List<Task> tasks = new List<Task>();

                    candles.ForEach(candle => tasks.Add(_candleRepository.InsertCandle(candle, asset.AssetName, timeFrame.TimeframeName)));
                    await Task.WhenAll(tasks);

                    var candleInsertedCount = tasks.Where(task => ((Task<bool>) task).Result == true).ToList().Count;
                    
                    if (candleInsertedCount == 0)
                    {
                        _logger.LogInformation("[{ClassName}] No Candles were inserted!", nameof(CreateCandleStickUseCase));
                        return;
                    }

                    _logger.LogInformation("[{ClassName}] Inserted {candlesCount} candles successfully for timeframe {timeframe}!", 
                        nameof(CreateCandleStickUseCase), candleInsertedCount, timeFrame.TimeframeName);
                    
                    await _bus.Publish<ICandleInserted>(new
                    {
                        CandlesQuantity = candles.Count,
                        Timeframe = timeFrame,
                        InsertionDate = DateTime.Now
                    });

                    _logger.LogInformation("[{ClassName}] Message {@message} published!", nameof(CreateCandleStickUseCase), nameof(ICandleInserted));


                });
            });
            
        }
    }
}