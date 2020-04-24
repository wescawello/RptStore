using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    interface ISort
    {
        [JsonPropertyName("sort")] 
        public Dictionary<string, string> Sort { get; set; }

    }
}
