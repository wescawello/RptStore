using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QReturnOrder:ReturnOrder,ISort
    {
        public List<DateTimeOffset> ReturnDateRange { get; set; }

        new public string ApplyNumber { get; set; }
        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }
    }
}