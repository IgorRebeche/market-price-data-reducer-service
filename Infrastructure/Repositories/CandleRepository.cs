using Domain;
using Domain.Repositories;
using Infrastructure.Database.MongoDb;
using Microsoft.Extensions.Logging;
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
        private ILogger<CandleRepository> _logger;
        private IMongoCollection<Candle> _candleCollection;

        public CandleRepository(ILogger<CandleRepository> logger, IOptions<MarketPriceLakeDatabaseConfiguration> mongoConfig)
        {
            var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.Database);
            _logger = logger;
        }

        public async Task<Candle?> GetLastCandle(string brokeName, string symbol, string timeFrame)
        {
            var collectionName = $"{brokeName}.{symbol}.{timeFrame}";
            _candleCollection = _mongoDatabase.GetCollection<Candle>(collectionName);

            var candles = await _candleCollection
                .Find(x => true)
                .SortByDescending(x => x.TimeStamp)
                .ToListAsync()
                .ConfigureAwait(false);

            return candles.Count > 1 ? candles.First() : null;
        }

        public async Task<bool> InsertCandle(Candle candle, string symbol, string timeFrame)
        {
            var options = new CreateIndexOptions { Unique = true };
            var indexKeysDefine = Builders<Candle>.IndexKeys
                .Ascending(indexKey => indexKey.TimeStamp)
                .Ascending(indexKey => indexKey.Volume)
                .Ascending(indexKey => indexKey.OpenPrice)
                .Ascending(indexKey => indexKey.ClosePrice)
                .Ascending(indexKey => indexKey.HighPrice)
                .Ascending(indexKey => indexKey.LowPrice);

            var indexModel = new CreateIndexModel<Candle>(indexKeysDefine, options);

            var collectionName = $"{candle.BrokerName}.{symbol}.{timeFrame}";
            _candleCollection = _mongoDatabase.GetCollection<Candle>(collectionName);
            
            try
            {
                await Task.WhenAll(_candleCollection.Indexes.CreateOneAsync(indexModel), _candleCollection.InsertOneAsync(candle));
                _logger.LogDebug("Candle Inserted!: {@candle}", candle);
                return true;

            } catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category.Equals(ServerErrorCategory.DuplicateKey))
                {
                    _logger.LogError("Chave duplicada, ignorando inserção. Candle {@candle}", candle);
                    return false;
                }

                throw;
            }

        }

        public Task InsertManyCandles(IEnumerable<Candle> candles, string symbol, string timeFrame)
        {
            throw new NotImplementedException();
        }
    }
}
