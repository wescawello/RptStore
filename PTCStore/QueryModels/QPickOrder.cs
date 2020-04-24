using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
namespace PTCStore.QueryModels
{
    public class QPickOrder: PickOrder,ISort
    {
        new public Guid PickOrderId { get; set; }
        new public string Keeper { get; set; }
        public bool AskPre { get; set; }
        public Guid[] BarcodeIds { get; set; }

        public FormateNumbr[] FormateNumbrs { get; set; }

        public bool ForSqle { get; set; }


        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }
    }
}