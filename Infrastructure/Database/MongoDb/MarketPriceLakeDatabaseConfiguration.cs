using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.MongoDb
{
    public class MarketPriceLakeDatabaseConfiguration
    {
        public string ConnectionString { get; set; } = null!;

        public string Database { get; set; } = null!;
        
        public string ConfigurationDatabase { get; set; } = null!;
        
    }
}
