using Moq;
using Xunit;
using Microsoft.Extensions.Options;
using Domain;
using System.Collections.Generic;
using AutoFixture;
using Application.Common;
using FluentAssertions;
using Domain.Options;
using Application.Services.Ticker;

namespace UnitTests
{
    public class CandleGenerationHelperTest
    {
        private Mock<IOptions<List<TimeframeOptions>>> _timeFrameOptions;
        private Fixture _fixture;

        public CandleGenerationHelperTest()
        {
            _timeFrameOptions = new Mock<IOptions<List<TimeframeOptions>>>();
            _fixture = new Fixture();
        }

        [Fact]
        public void ShouldGenerateCandlesSuccessfully()
        {
            //var timeFrameOptions = _fixture.Create<List<Timeframe>>();
            var timeFrameOptions = new List<TimeframeOptions>();
            timeFrameOptions.Add(new TimeframeOptions { TimeframeInSeconds = 5, TimeframeName = "Custom"});
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);

            var tickers = GetTickers(new List<decimal> { 40, 120, 90, 30, 100, 75, 60 });
            var candleGenerationHelper = new CandleGenerationHelper();
            var candle = candleGenerationHelper.ProcessCandles(timeFrameOptions[0], tickers);

            Assert.Equal(40, candle[0].OpenPrice);
            Assert.Equal(120, candle[0].HighPrice);
            Assert.Equal(30, candle[0].LowPrice);
            Assert.Equal(75, candle[0].ClosePrice);
        }

        [Fact]
        public void ShouldNotGenerateCandles()
        {
            // Arrange
            var timeFrameOptions = new List<TimeframeOptions>();
            timeFrameOptions.Add(new TimeframeOptions { TimeframeInSeconds = 5, TimeframeName = "Custom" });
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);

            // Act
            var tickers = GetTickers(new List<decimal> { 40, 120, 90 });
            var candleGenerationHelper = new CandleGenerationHelper();
            var candle = candleGenerationHelper.ProcessCandles(timeFrameOptions[0], tickers);

            // Assert
            candle.Should().BeEmpty();
        }
        private List<TickerResponse> GetTickers(List<decimal> fakePrices)
        {
            var tickers = new List<TickerResponse>();
            var i = 0;
            fakePrices.ForEach(x =>
            {
                tickers.Add(new TickerResponse { BrokerName = "Binance", Symbol = "BTCUSD", Price = x, Volume = 100, Timestamp = 100 + i });
                i++;
            });
            return tickers;
            
        }
    }
}