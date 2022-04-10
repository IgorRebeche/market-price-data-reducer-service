using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Xunit.Sdk;
using Microsoft.Extensions.Logging;
using Application.UseCases.CreateCandlestick;
using Microsoft.Extensions.Options;
using Domain;
using System.Collections.Generic;
using AutoFixture;

namespace UnitTests
{
    [TestClass]
    public class CreateCandleStickUseCaseTest
    {
        private Mock<ILogger<CreateCandleStickUseCase>> _logger;
        private Mock<IOptions<List<Timeframe>>> _timeFrameOptions;
        private CreateCandleStickUseCase _useCase;
        private Fixture _fixture;

        public CreateCandleStickUseCaseTest()
        {
            _logger = new Mock<ILogger<CreateCandleStickUseCase>>();
            _timeFrameOptions = new Mock<IOptions<List<Timeframe>>>();
            _useCase = new CreateCandleStickUseCase(_logger.Object,  _timeFrameOptions.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void ShouldProcess()
        {
            var timeFrameOptions = _fixture.Create<List<Timeframe>>();
            _timeFrameOptions.Setup(x => x.Value).Returns(timeFrameOptions);
            _useCase.ExecuteAsync();
        }
    }
}