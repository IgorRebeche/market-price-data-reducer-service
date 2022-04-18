using Domain;
using Domain.Repositories;
using Infrastructure.Database.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CandleRepository: ICandleRepository
    {
        private IMongoDatabase _mongoDatabase;

        public CandleRepository(IOptions<MarketPriceLakeDatabaseConfiguration> mongoConfig)
        {
            var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.Database);
        }

        public Task<Candle> GetLastCandle(string brokeName, string symbol, string timeFrame)
        {
            throw new NotImplementedException();
        }

        public Task InsertCandle(Candle candle)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyCandles(IEnumerable<Candle> candles)
        {
            throw new NotImplementedException();
        }
    }
}
