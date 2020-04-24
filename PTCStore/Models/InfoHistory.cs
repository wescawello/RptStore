using System;
using System.Collections.Generic;
using System.Text;

namespace PTCStore.Models
{
    public class InfoHistory
    {
        public Guid InfoHistoryId { get; set; }
        public string ProdName { get; set; }
        public string StockUnitName { get; set; }
        public int StockUnitId { get; set; }
        public int FormateId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }

        public string TypeName { get; set; }

        public string CreateYM { get; set; }
    }
}
