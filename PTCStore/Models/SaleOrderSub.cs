using System;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class SaleOrderSub
    {
        public Guid SaleOrderSubId { get; set; }
        public Guid SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }


        public Guid BarcodeId { get; set; }
        
        [JsonIgnore]
        public virtual Barcode Barcode { get; set; }

        public bool Returned { get; set; }

 
    }
}