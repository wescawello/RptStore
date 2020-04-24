using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QInfo :ISort
    {
        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }
        public bool ByProd { get; set; }
        public int? FormateId { get; set; }
        public int? StockUnitId { get; set; }
        
    }
}
