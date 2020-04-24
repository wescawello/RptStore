using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class Barcode
    {
        public Guid BarcodeId { get; set; }

 
        [StringLength(100, MinimumLength = 1)]
        public string BarcodeValue { get; set; }
        [JsonIgnore]
        public Purchase Purchase { get; set; }
        public Guid PurchaseId { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string Support { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }

        public int StockUnitId { get; set; }  
        public bool Picked { get; set; } = false;
        public bool Saled { get; set; } = false;
        public int ReturnCount { get; set; } = 0;
        [JsonIgnore]
        public StockUnit StockUnit { get; set; }
    }
}