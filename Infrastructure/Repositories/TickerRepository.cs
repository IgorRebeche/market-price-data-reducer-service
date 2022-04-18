using Domain;
using Domain.Repositories;
using Infrastructure.Database.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TickerRepository: ITickerRepository
    {
        private IMongoCollection<Ticker>? _tickersCollection;
        private IMongoDatabase _mongoDatabase;

        public TickerRepository(IOptions<MarketPriceLakeDatabaseConfiguration> mongoConfig)
        {
            var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.Database);
        }

        public async Task<IEnumerable<Ticker>> GetTickers(string brokerName, string symbol, string timeFrame, long timeStampFrom)
        {
            var collectionName = $"{brokerName}.{symbol}.RAW";
            _tickersCollection = _mongoDatabase.GetCollection<Ticker>(collectionName);

            var tickers = await _tickersCollection.FindAsync(ticker => ticker.Timestamp >= timeStampFrom).ConfigureAwait(false);
            
            return tickers.ToList();
        }

        public async Task<IEnumerable<Ticker>> GetTickersRange(string brokerName, string symbol, string timeFrame, long timeStampFrom, long timeStampTo)
        {
            var collectionName = $"{brokerName}.{symbol}.RAW";
            _tickersCollection = _mongoDatabase.GetCollection<Ticker>(collectionName);
            var filterBuilder = Builders<Ticker>.Filter;//.And(ticker => ticker.Timestamp >= timeStampFrom && ticker.Timestamp <= timeStampTo, true);
            var filter = filterBuilder.Gte(x => x.Timestamp, timeStampFrom) & filterBuilder.Lte(x => x.Timestamp, timeStampTo);
            //var filter = new ObjectId("6254b2aada268537f778acb7");

            var tickers = await _tickersCollection
                .Find(filter)
                .SortBy(x => x.Timestamp)
                .ToListAsync()
                .ConfigureAwait(false);

            return tickers.ToList();
        }
    }
}
