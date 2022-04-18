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

namespace UnitTests
{
    public class CreateCandleStickUseCaseTest
    {
        private Mock<ILogger<CreateCandleStickUseCase>> _logger;
        private Mock<IOptions<List<Timeframe>>> _timeFrameOptions;
        private Mock<IOptions<List<Asset>>> _assetOptions;
        private Mock<ITickerRepository> _tickerRepository;
        private Mock<ICandleRepository> _candleRepository;
        private CreateCandleStickUseCase _useCase;
        private Fixture _fixture;

        public CreateCandleStickUseCaseTest()
        {
            _logger = new Mock<ILogger<CreateCandleStickUseCase>>();
            _timeFrameOptions = new Mock<IOptions<List<Timeframe>>>();
            _assetOptions = new Mock<IOptions<List<Asset>>>();
            _tickerRepository = new Mock<ITickerRepository>();
            _candleRepository = new Mock<ICandleRepository>();
            _useCase = new CreateCandleStickUseCase(_logger.Object, 
                _timeFrameOptions.Object, _assetOptions.Object, _tickerRepository.Object, _candleRepository.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ShouldGenerateAndSaveOneCandle()
        {
            //Arrange
            var timeFrameOptions = new List<Timeframe>();
            timeFrameOptions.Add(new Timeframe { TimeframeInSeconds = 5, TimeframeName = "Custom"});
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);
            
            var assetsOptions = new List<Asset>();
            assetsOptions.Add(new Asset { AssetName = "BTCUSD", BrokerName = "Binance" });
            _assetOptions.Setup(o => o.Value).Returns(assetsOptions);

            _candleRepository.Setup(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_fixture.Create<Candle>());
            
            var tickers = GetTickers(new List<decimal> { 40, 120, 90, 50, 100, 75, 60 });
            _tickerRepository.Setup(x => x.GetTickers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(tickers);

            //Act
            await _useCase.ExecuteAsync();

            //Assert
            _tickerRepository.Verify(x => x.GetTickers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()), Times.Once);
            _candleRepository.Verify(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _candleRepository.Verify(x => x.InsertCandle(It.IsAny<Candle>()), Times.Once);

        }

        [Fact]
        public async Task ShouldGenerateAndSaveManyCandles()
        {
            //var timeFrameOptions = _fixture.Create<List<Timeframe>>();
            var timeFrameOptions = new List<Timeframe>();
            timeFrameOptions.Add(new Timeframe { TimeframeInSeconds = 5, TimeframeName = "Custom" });
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);

            var assetsOptions = new List<Asset>();
            assetsOptions.Add(new Asset { AssetName = "BTCUSD", BrokerName = "Binance" });
            _assetOptions.Setup(o => o.Value).Returns(assetsOptions);

            _candleRepository.Setup(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_fixture.Create<Candle>());

            var tickers = GetTickers(new List<decimal> { 40, 120, 90, 50, 100, 75, 60, 100, 101, 108, 90 });
            _tickerRepository.Setup(x => x.GetTickers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(tickers);


            await _useCase.ExecuteAsync();

            _tickerRepository.Verify(x => x.GetTickers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()), Times.Once);
            _candleRepository.Verify(x => x.GetLastCandle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _candleRepository.Verify(x => x.InsertCandle(It.IsAny<Candle>()), Times.Exactly(2));

        }

        private Ticker[] GetTickers(List<decimal> fakePrices)
        {
            var tickers = new List<Ticker>();
            var i = 0;
            fakePrices.ForEach(x =>
            {
                tickers.Add(new Ticker { BrokerName = "Binance", Symbol = "BTCUSD", Price = x, Volume = 100, Timestamp = 100 + i });
                i++;
            });
            return tickers.ToArray();
            
        }
    }
}