using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
    public class QDevice : ISort
    {

        public string Name { get; set; }
        public bool? Display { get; set; }
        [JsonPropertyName("sort")]

        public Dictionary<string, string> Sort { get; set; }
 
    }
}
