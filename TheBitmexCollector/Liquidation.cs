using System;
using System.Collections.Generic;
using System.Text;

namespace TheBitmexCollector
{
    public class Liquidation
    {
        public string LiquidationId { get; set; }
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public string Direction { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
