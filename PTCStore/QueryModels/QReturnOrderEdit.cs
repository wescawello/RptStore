using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QReturnOrderEdit : ISort
    {
 

        public Guid ReturnOrderId { get; set; }

        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }
    }
}