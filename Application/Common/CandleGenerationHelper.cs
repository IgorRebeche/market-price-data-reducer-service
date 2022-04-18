using Domain;
using Domain.Options;

namespace Application.Common
{
    public class CandleGenerationHelper
    {
        public List<Candle> ProcessCandles(Timeframe timeframe, List<Ticker> tickers)
        {
            var tickerTimeRangeInSeconds = tickers.Last().Timestamp - tickers.First().Timestamp;
            var tickersRangeIsShorterThanTimeFrame = tickerTimeRangeInSeconds < timeframe.TimeframeInSeconds;

            if (tickersRangeIsShorterThanTimeFrame) return new List<Candle>();

            long tmpStartTime = 0;
            long tmpStopTime = 0;
            int tickerCount = 0;
            int count = 0;
            List<Candle> candles = new List<Candle>();

            for (int i = 0; i < tickers.Count; i++)
            {
                int actualCandleCount = candles.Count();

                var actualTicker = tickers[i];
                var TimeframeDistance = actualTicker.Timestamp % timeframe.TimeframeInSeconds;
                var TimeFrameDistanceToleranceOffset = 5000;

                // Pegar tickers dentro do timebox
                if (TimeframeDistance < TimeFrameDistanceToleranceOffset && actualTicker.Timestamp > tmpStopTime)
                {
                    tmpStartTime = actualTicker.Timestamp - TimeframeDistance;
                    tmpStopTime = tmpStartTime + timeframe.TimeframeInSeconds;

                    DateTime candleTime = DateTimeOffset.FromUnixTimeMilliseconds(tmpStartTime).DateTime;

                    candles.Add(new Candle());

                    actualCandleCount = candles.Count();

                    if (actualCandleCount > 1)
                    {
                        var lastTicker = tickers[i - 1];
                        candles[actualCandleCount - 2].ClosePrice = lastTicker.Price;
                        candles[actualCandleCount - 2].TickerCount = tickerCount;

                    };
                    tickerCount = 0;
                    candles[actualCandleCount - 1].TimeStamp = tmpStartTime;
                    candles[actualCandleCount - 1].Date = candleTime;
                    candles[actualCandleCount - 1].OpenPrice = candles[actualCandleCount - 1].HighPrice = candles[actualCandleCount - 1].LowPrice = actualTicker.Price;
                }

                if (actualCandleCount == 0) continue;


                if (candles[actualCandleCount - 1].HighPrice < actualTicker.Price)
                {
                    candles[actualCandleCount - 1].HighPrice = actualTicker.Price;
                }

                if (candles[actualCandleCount - 1].LowPrice > actualTicker.Price)
                {
                    candles[actualCandleCount - 1].LowPrice = actualTicker.Price;
                }
                tickerCount++;
                count++;


            }
            
            if (candles.Last().ClosePrice == 0) candles.RemoveAt(candles.Count() - 1);

            return candles;
        }
    }
}
