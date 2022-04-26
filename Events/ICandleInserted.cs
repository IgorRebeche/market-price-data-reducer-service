using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public interface ICandleInserted
    {
        public int CandlesQuantity { get; set; }
        public string Timeframe { get; set; }
        public DateTime InsertionDate { get; set; }
    }
}
