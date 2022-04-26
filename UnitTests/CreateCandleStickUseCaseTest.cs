using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Application.UseCases.CreateCandlestick;
using Microsoft.Extensions.Options;
using Domain;
using System.Collections.Generic;
using AutoFixture;
using Domain.Repositories;
using System.Threading.Tasks;
using FluentAssertions;
using Domain.Options;
using Application.Services.Ticker;
using MassTransit;

namespace UnitTests
{
    public class CreateCandleStickUseCaseTest
    {
        private Mock<ILogger<CreateCandleStickUseCase>> _logger;
        private Mock<IOptions<List<TimeframeOptions>>> _timeFrameOptions;
        private Mock<IOptions<List<AssetOptions>>> _assetOptions;
        private Mock<ITickerService> _tickerService;
        private Mock<ICandleRepository> _candleRepository;
        private Mock<IBus> _bus;
        private CreateCandleStickUseCase _useCase;
        private Fixture _fixture;

        public CreateCandleStickUseCaseTest()
        {
            _logger = new Mock<ILogger<CreateCandleStickUseCase>>();
            _timeFrameOptions = new Mock<IOptions<List<TimeframeOptions>>>();
            _assetOptions = new Mock<IOptions<List<AssetOptions>>>();
            _tickerService = new Mock<ITickerService>();
            _candleRepository = new Mock<ICandleRepository>();
            _bus = new Mock<IBus>();
            _useCase = new CreateCandleStickUseCase(_logger.Object, 
                _timeFrameOptions.Object, _assetOptions.Object, _tickerService.Object, _candleRepository.Object, _bus.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ShouldGenerateAndSaveOneCandle()
        {
            //Arrange
            var timeFrameOptions = new List<TimeframeOptions>();
            timeFrameOptions.Add(new TimeframeOptions { TimeframeInSeconds = 5, TimeframeName = "Custom"});
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);
            
            var assetsOptions = new List<AssetOptions>();
            assetsOptions.Add(new AssetOptions { AssetName = "BTCUSD", BrokerName = "Binance" });
            _assetOptions.Setup(o => o.Value).Returns(assetsOptions);

            _candleRepository.Setup(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_fixture.Create<Candle>());
            
            var tickers = GetTickers(new List<decimal> { 40, 120, 90, 50, 100, 75, 60 });
            _tickerService.Setup(x => x.GetTickers(It.IsAny<TickerInput>())).ReturnsAsync(tickers);

            //Act
            await _useCase.ExecuteAsync();
            //Assert
            _tickerService.Verify(x => x.GetTickers(It.IsAny<TickerInput>()), Times.Once);
            _candleRepository.Verify(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _candleRepository.Verify(x => x.InsertCandle(It.IsAny<Candle>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task ShouldGenerateAndSaveManyCandles()
        {
            //var timeFrameOptions = _fixture.Create<List<Timeframe>>();
            var timeFrameOptions = new List<TimeframeOptions>();
            timeFrameOptions.Add(new TimeframeOptions { TimeframeInSeconds = 5, TimeframeName = "Custom" });
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);

            var assetsOptions = new List<AssetOptions>();
            assetsOptions.Add(new AssetOptions { AssetName = "BTCUSD", BrokerName = "Binance" });
            _assetOptions.Setup(o => o.Value).Returns(assetsOptions);

            _candleRepository.Setup(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_fixture.Create<Candle>());

            var tickers = GetTickers(new List<decimal> { 40, 120, 90, 50, 100, 75, 60, 100, 101, 108, 90 });
            _tickerService.Setup(x => x.GetTickers(It.IsAny<TickerInput>())).ReturnsAsync(tickers);

            await _useCase.ExecuteAsync();

            _tickerService.Verify(x => x.GetTickers(It.IsAny<TickerInput>()), Times.Once);
            _candleRepository.Verify(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _candleRepository.Verify(x => x.InsertCandle(It.IsAny<Candle>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        private TickerResponse[] GetTickers(List<decimal> fakePrices)
        {
            var tickers = new List<TickerResponse>();
            var i = 0;
            fakePrices.ForEach(x =>
            {
                tickers.Add(new TickerResponse { BrokerName = "Binance", Symbol = "BTCUSD", Price = x, Volume = 100, Timestamp = 100 + i });
                i++;
            });
            return tickers.ToArray();
            
        }
    }
}