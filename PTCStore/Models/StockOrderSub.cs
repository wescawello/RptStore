using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
   public class StockOrderSub
    {
        public Guid StockOrderSubId { get; set; }

        public Guid StockOrderId { get; set; }

        public Guid BarcodeId { get; set; }
        [JsonIgnore]
        public virtual Barcode Barcode { get; set; }
        [JsonIgnore] 
        public virtual StockOrder StockOrder { get; set; }

    }
}
