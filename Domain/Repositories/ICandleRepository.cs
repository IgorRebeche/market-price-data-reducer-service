using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICandleRepository
    {
        public Task<bool> InsertCandle(Candle candle, string symbol, string timeFrame);

        public Task InsertManyCandles(IEnumerable<Candle> candles, string symbol, string timeFrame);

        public Task<Candle?> GetLastCandle(string brokeName, string symbol, string timeFrame);
    }
}
