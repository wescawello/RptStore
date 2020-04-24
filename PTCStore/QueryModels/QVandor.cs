using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QVandor : ISort
    {


        public bool? Status { get; set; }
        public bool? TaxStatus { get; set; }

        [JsonPropertyName("sort")]

        public Dictionary<string, string> Sort { get; set; }
    }
}