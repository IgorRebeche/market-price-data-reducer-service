using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICandleRepository
    {
        public Task InsertCandle(Candle candle);

        public Task InsertManyCandles(IEnumerable<Candle> candles);

        public Task<Candle> GetLastCandle(string brokeName, string symbol, string timeFrame);
    }
}
