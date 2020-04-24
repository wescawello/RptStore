using System;
using System.Collections.Generic;
 
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class Formate
    {
 

        public int FormateId { get; set; }
        public string Name { get; set; }


        public int? SetCount { get; set; }

        [JsonPropertyName("PurchasePrice")]
        public double? PurchasePriceDefault { get; set; }
        [JsonPropertyName("SalePrice")]
        public double? SalePriceDefault { get; set; }
        
        public bool Status { get; set; } = true;
        public bool DoTimes { get; set; }
        public bool WithBarcodes { get; set; }
        
        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        public string UpDateId { get; set; }
        [JsonIgnore]
        public VandorDevice VandorDevice { get; set; }
 


        [JsonIgnore]
        public List<Purchase> Purchases { get; set; }


        [JsonPropertyName("VandorId")]

        public int VandorId { get;  set; }
        [JsonPropertyName("DeviceId")]
        public int DeviceId { get;  set; }
    }
}