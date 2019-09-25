using System;
using System.Collections.Generic;
using System.Text;

namespace TheBitmexCollector
{
    public class TradeBin
    {
        public int TradeBinId { get; set; }
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal HomeNotional { get; set; }
        public decimal ForeignNotional { get; set; }
        public decimal Volume { get; set; }
        public decimal Trades { get; set; }

    }
}
