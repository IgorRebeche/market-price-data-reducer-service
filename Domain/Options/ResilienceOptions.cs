using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Options
{
    public class ResilienceOptions
    {
        public int Retry { get; set; }
        public int Wait { get; set; }
        public int Timeout { get; set; }
    }
}
