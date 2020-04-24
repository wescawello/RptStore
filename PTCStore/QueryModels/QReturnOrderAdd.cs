using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QReturnOrderAdd:ISort
    {
        public Customer Customer { get;  set; }

        public List<Guid> PickOrderSubIds { get; set; }

        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }
    }
}