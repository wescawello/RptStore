using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QStockUnit :  StockUnit, ISort
    {
        new public string Name { get; set; }
      
        new public string Keeper { get; set; }

        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }


       
    }
}
