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

        public CreateCandleStickUseCase(ILogger<CreateCandleStickUseCase> logger,IOptions<List<Timeframe>> timeFrameOptions) 
        {
            _timeFrameOptions = timeFrameOptions;
            _logger = logger;
        }
        public Task ExecuteAsync()
        {

            // TODO: Pegar a ultima candle registrado para o timeframe
            
            // TODO: Persistir cotações de um range de preço RAW

            // TODO: Criar candles disponiveis no range

            _timeFrameOptions.Value.ForEach(async x => {
                _logger.LogInformation("Timeframe Name: {TimeframeName} | Timeframe Time in Miliseconds: {timeframeInMilli}",
                 x.TimeframeName, x.TimeframeInSeconds);
            });
            throw new NotImplementedException();
        }
    }
}