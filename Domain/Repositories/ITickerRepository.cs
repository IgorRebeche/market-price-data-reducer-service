using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ITickerRepository
    {
        public Task<IEnumerable<Ticker>> GetTickers(string brokerName, string symbol, string timeFrame, long timeStampFrom);
        public Task<IEnumerable<Ticker>> GetTickersRange(string brokerName, string symbol, string timeFrame, long timeStampFrom, long timeStampTo);
    }
}
