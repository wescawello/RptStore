using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QPurchase : Purchase, ISort
    {
        public List<DateTimeOffset> ApplyDateRange { get; set; }
        public List<DateTimeOffset> PurchaseDateRange { get; set; }

        new public string ApplyNumber { get; set; }

        public double? TotalMax { get; set; }
        public double? TotalMin { get; set; }
        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get;set; }
    }
}
