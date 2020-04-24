using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QFormate : ISort
    {

        public string Name { get; set; }
        public int? SetCount { get; set; }
        public bool? Status { get; set; }
        public bool? DoTimes { get; set; }
        public bool? WithBarcodes { get; set; }
        public double? PurchasePriceDefault { get; set; }
        public double? SalePriceDefault { get; set; }

        [JsonPropertyName("sort")]

        public Dictionary<string, string> Sort { get; set; }
 
    }
}
